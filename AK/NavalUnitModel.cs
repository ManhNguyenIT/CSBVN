using MudBlazor;

namespace AK;

public class NavalUnitModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public NavalUnitModel? Parent { get; set; }

    public bool IsAffiliateLoaded { get; set; }
    public bool IsExpanded { get; set; }
    public bool HasChild { get; set; }
    public bool IsSelected { get; set; }
    public HashSet<NavalUnitModel> AffiliatedUnits { get; set; } = new HashSet<NavalUnitModel>();
}
