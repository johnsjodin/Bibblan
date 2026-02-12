namespace Bibblan.Core;

public interface ISearchable
{
    bool Matches(string searchTerm);
}
