# File Format

Alphabet Soup accepts word search puzzles in a simple text format.

## Format Specification

A word search file consists of two sections separated by a blank line:

1. **Letter Grid** - The puzzle grid with one row per line
2. **Word List** - One word per line to search for

## Example

```
A B C D E
F G H I J
K L M N O
P Q R S T
U V W X Y

ABC
KLM
CGK
EJO
```

## Rules

### Letter Grid

- Each row should have the same number of letters
- Letters can be separated by spaces or written consecutively
- Both uppercase and lowercase letters are accepted

### Word List

- One word per line
- Words can contain spaces (they will be searched without spaces)
- Empty lines are ignored

## Examples with Spaces

Words with spaces like "HELLO WORLD" will be searched as "HELLOWORLD" in the grid.

```
H E L L O W O R L D
A B C D E F G H I J

HELLO WORLD
```

## File Extensions

The application accepts files with these extensions:
- `.txt`
- `.wordsearch`
