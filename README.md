this is a test title
====
# CSharpHelpers

* [FOREWORD](#foreword)
* [STRINGS](#strings)
 ** [Trim leading and trailing white-space from string](#Trim leading and trailing white-space from string)

# FOREWORD

A collection of pure `bash` alternatives to external processes and programs. The `bash` scripting language is more powerful than people realise and most tasks can be accomplished without depending on external programs.

# STRINGS

## Trim leading and trailing white-space from string

This is an alternative to `sed`, `awk`, `perl` and other tools. The
function below works by finding all leading and trailing white-space and
removing it from the start and end of the string. The `:` built-in is used in place of a temporary variable.
