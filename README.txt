Version 1: 21-01-2026

This codebase is s PoC seachengine that consist of two programs and a class library.

The two programs are the indexer (also called a crawler) and a search program. Both
are simple console programs.

The indexer will crawl a folder (in depth) and create a reverse index
in a database. It will only index text files with .txt as extension. See the Config.cs for the folder
to be indexed.

The search program (see the ConsoleSearch project) offers a query-based search
in the reverse index.

The class library Shared contains classes that are used by the indexer
and the ConsoleSearch. It contains:

- Paths containing a static path to the database (used by both the indexer (write-only), and
the search program (read-only).
- BEDocument (BE for Business Entity) - a class representing a document.

Support for using either Sqlite or Postgres as database engine.





