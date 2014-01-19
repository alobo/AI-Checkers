AI-Checkers
===========

Artificial Intelligence for Checkers written in C#

Algorithms
------

#### AI_Tree
This AI implementation:

1. Recursively builds a game tree of n-plies, where n is specified by AI_MAXPLYLEVEL. (By default, this field has a value of 2).

2. Scores each move based on weighted criteria. 

3. Sums the branch scores

4. Executes the move with the highest score.


#### Utils.GetOpenSquares(Square[,] Board, Point checker)
This function recursively finds all valid moves on the given board with the selected checker. Follows standard checker rules for single and multi-piece captures and handles several edge cases (mainly infinite move loops).
