using SelectAid.Models;

namespace SelectAid.Services;

public class PredictionService
{
    public IReadOnlyList<string> GetCandidates(string current, HistoryStore history, UserDictionary dict)
    {
        var candidates = new List<string>();
        if (string.IsNullOrWhiteSpace(current))
        {
            return candidates;
        }
        var recent = history.Items
            .Where(i => i.Text.StartsWith(current, StringComparison.OrdinalIgnoreCase))
            .Select(i => i.Text)
            .Distinct()
            .Take(3)
            .ToList();
        candidates.AddRange(recent);
        var fromDict = dict.Words
            .Where(w => w.StartsWith(current, StringComparison.OrdinalIgnoreCase))
            .Distinct()
            .Take(8 - candidates.Count)
            .ToList();
        candidates.AddRange(fromDict);
        return candidates.Take(8).ToList();
    }
}
