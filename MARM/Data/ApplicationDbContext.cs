using Microsoft.EntityFrameworkCore;
namespace MARM.Data;

public class ApplicationDbContext : DbContext
{
    private DbSet<NavalUnit> NavalUnits { get; set; }
    private DbSet<Mission> Missions { get; set; }
    private DbSet<BoatUnitMission> BoatUnitMissions { get; set; }
    private DbSet<BoatUnitShot> BoatUnitShots { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public async Task<IEnumerable<NavalUnit>> GetNavalUnits(Guid parentId) => await NavalUnits.Where(u => u.ParentId == parentId).ToListAsync();

    public async Task<Result<NavalUnit>> CreateNavalUnit(Guid parentId, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return new Result<NavalUnit>()
            {
                IsSuccess = false,
                Message = $"Tên đơn vị không được để trống",
            };
        }

        if (await NavalUnits.AnyAsync(u => u.ParentId == parentId && u.Name == name))
        {
            return new Result<NavalUnit>()
            {
                IsSuccess = false,
                Message = $"Tên đơn vị {name} đã tồn tại",
            };
        }

        var entry = await NavalUnits.AddAsync(new NavalUnit { ParentId = parentId, Name = name, });
        await SaveChangesAsync();

        return new Result<NavalUnit>()
        {
            IsSuccess = true,
            Message = $"Tạo thành công đơn vị {name}",
            Value = new NavalUnit { ParentId = parentId, Name = name, Id = entry.Entity.Id },
        };
    }

    public async Task<Result> DeleteNavalUnit(Guid id)
    {
        var unit = await NavalUnits.FindAsync(id);
        if (unit == null) return new Result() { IsSuccess = false, Message = $"Không tìm thấy dữ liệu đơn vị cần xóa, ID = {id}", };

        var children = await NavalUnits.Where(u => u.ParentId == unit.Id).ToListAsync();

        if (children.Any())
        {
            foreach (var child in children)
            {
                var result = await DeleteNavalUnit(child.Id);
                if (!result.IsSuccess)
                {
                    return new Result() { IsSuccess = false, Message = $"Lỗi xóa đơn vị trực thuộc {child.Name}.\n Message: {result.Message}", };
                }
            }
        }

        NavalUnits.Remove(unit);
        await SaveChangesAsync();
        return new Result() { IsSuccess = true, Message = $"Xóa thành công đơn vị {unit.Name}", };
    }

    public async Task<IEnumerable<Mission>> GetNavalMissions(Guid id)
    {
        var missions = await Missions.Where(u => u.NavalUnitId == id).ToListAsync();
        foreach (var mission in missions)
        {
            mission.BoatCount = await BoatUnitMissions.CountAsync(b => b.MissionId == mission.Id);
        }

        return missions;
    }

    public async Task<Mission?> GetNavalMissionById(Guid id) => await Missions.FindAsync(id);

    public async Task<Result<Mission>> CreateNewNavalMission(Guid unitId, string name, DateTime startAt, string note)
    {
        if (string.IsNullOrEmpty(name))
        {
            return new Result<Mission>()
            {
                IsSuccess = false,
                Message = $"Tên nhiệm vụ không được để trống",
            };
        }

        if (await Missions.AnyAsync(m => m.NavalUnitId == unitId && m.Name == name))
        {
            return new Result<Mission> { IsSuccess = false, Message = "Tên nhiệm vụ đã tồn tại", };
        }

        var entry = await Missions.AddAsync(new Mission()
        {
            NavalUnitId = unitId,
            Name = name,
            State = Enums.MissionState.Create,
            CreateAt = DateTime.Now,
            StartAt = startAt,
            ModifiedAt = DateTime.Now,
            Note = note,
        });

        await SaveChangesAsync();
        return new Result<Mission> { IsSuccess = true, Message = $"Tạo nhiệm vụ {name} thành công.", Value = entry.Entity, };
    }

    public async Task<Result> DeleteNavalMission(Guid id)
    {
        var mission = await Missions.FindAsync(id);
        if (mission == null) return new Result() { IsSuccess = false, Message = $"Không tìm thấy dữ liệu nhiệm vụ cần xóa, ID = {id}", };

        Missions.Remove(mission);
        await SaveChangesAsync();
        return new Result() { IsSuccess = true, Message = $"Xóa thành công nhiệm vụ {mission.Name}", };
    }

    private static readonly Random rnd = new();
    public async Task<IEnumerable<BoatUnitMission>> GetBoatUnitMissions(Guid id)
    {
        var boards = await BoatUnitMissions.Where(u => u.MissionId == id).ToListAsync();
        foreach (var board in boards)
        {
            for(int i = 0; i < board.ShotCounts.Length; i++)
            {
                board.ShotCounts[i] = rnd.Next(0, 25);
                board.ShotTotal += board.ShotCounts[i];
            }
        }

        return boards;
    }

    public async Task<Result<BoatUnitMission>> CreateNewBoatUnitMission(Guid missionId, string name, string note)
    {
        if (string.IsNullOrEmpty(name))
        {
            return new Result<BoatUnitMission>()
            {
                IsSuccess = false,
                Message = $"Tên đơn vị tàu không được để trống",
            };
        }

        if (await BoatUnitMissions.AnyAsync(b => b.MissionId == missionId && b.Name == name))
        {
            return new Result<BoatUnitMission>
            {
                IsSuccess = false,
                Message = "Tên đơn vị tàu đã tồn tại",
            };
        }

        var entry = await BoatUnitMissions.AddAsync(new BoatUnitMission()
        {
            MissionId = missionId,
            Name = name,
            Note = note,
        });

        await SaveChangesAsync();
        return new Result<BoatUnitMission> { IsSuccess = true, Message = "", Value = entry.Entity, };
    }

    public async Task<Result> DeleteBoatUnitMission(Guid id)
    {
        var boat = await BoatUnitMissions.FindAsync(id);
        if (boat == null) return new Result() { IsSuccess = false, Message = $"Không tìm thấy dữ liệu đơn vị tàu cần xóa, ID = {id}", };

        BoatUnitMissions.Remove(boat);
        await SaveChangesAsync();
        return new Result() { IsSuccess = true, Message = $"Xóa thành công đơn vị tàu {boat.Name}", };
    }

    public async Task<Result> UpdateBoatUnitMissionInfo(Guid id, string name, string note)
    {
        if(string.IsNullOrEmpty(name)) return new Result() { IsSuccess = false, Message = "Tên đơn vị tàu không được để trống", };

        var boat = await BoatUnitMissions.FindAsync(id);
        if (boat == null) return new Result() { IsSuccess = false, Message = $"Không tìm thấy dữ liệu đơn vị tàu cần update, ID = {id}", };

        if(await BoatUnitMissions.AnyAsync(b => b.MissionId == boat.MissionId && b.Name == name && b.Id != boat.Id))
        {
            return new Result() { IsSuccess = false, Message = $"Tên đơn vị tàu {name} bị trùng", };
        }

        boat.Name = name;
        boat.Note = note;
        await SaveChangesAsync();
        return new Result() { IsSuccess = true, Message = "", };
    }
}

