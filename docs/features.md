# Features

Alphabet Soup includes several features to help you solve word search puzzles efficiently.

## Word Finding

The solver searches for words in all eight directions:

- **Horizontal** - Left to right and right to left
- **Vertical** - Top to bottom and bottom to top
- **Diagonal** - All four diagonal directions

Words are found automatically when a puzzle is loaded. The results show the word, starting position, direction, and ending position.

## Visual Highlighting

### Cell Highlighting

When you click on a word or answer:
- The cells containing that word are highlighted with a color
- Multiple matches for the same word are shown in different colors
- Click "Clear Highlights" to reset

### Circle Overlay

Toggle "Circle Answers" to show stadium-shaped outlines around all found words:
- Each answer gets a unique color from the palette
- Hover over a circle to highlight just that word
- The word list and answer list update to show the hovered word

## Words to Find Panel

The left sidebar shows:
- All words from the puzzle's word list
- Words are sorted alphabetically
- Click any word to highlight it in the grid
- Clicking scrolls the corresponding answer into view

## Answers Panel

Below the words list:
- Shows all found answers with coordinates
- Format: `WORD @ (row, col) Direction (endRow, endCol)`
- Click an answer to highlight just that specific match
- Hover highlighting syncs with circle hovers

## Clear Highlights

The "Clear Highlights" button resets:
- Selected word
- All cell highlighting
- Answer highlighting
- Hover state
