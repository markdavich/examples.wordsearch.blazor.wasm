using WordSearchSolver.Domain.Entities;

namespace WordSearchSolver.Application.Interfaces;

/// <summary>
/// Interface for finding words in a specific direction.
/// </summary>
public interface IDirectionalFinder
{
    /// <summary>
    /// Finds words in the configured direction.
    /// </summary>
    IEnumerable<Match> Find(char[,] grid, IEnumerable<string> words);
}
