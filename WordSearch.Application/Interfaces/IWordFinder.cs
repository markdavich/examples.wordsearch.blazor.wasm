using WordSearch.Domain.Entities;

namespace WordSearch.Application.Interfaces;

/// <summary>
/// Interface for finding words in a letter grid.
/// </summary>
public interface IWordFinder
{
    /// <summary>
    /// Finds all occurrences of the given words in the grid.
    /// </summary>
    /// <param name="grid">The letter grid to search.</param>
    /// <param name="words">The words to find.</param>
    /// <returns>All matches found.</returns>
    IEnumerable<Match> FindWords(char[,] grid, IEnumerable<string> words);
}
