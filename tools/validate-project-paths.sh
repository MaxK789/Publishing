#!/bin/bash
set -euo pipefail

missing=0
repo_root="$(git rev-parse --show-toplevel)"

check_solution() {
  grep -oE '"[^" ]+\\[^" ]+\.csproj"' Publishing.sln | tr -d '"' | while IFS= read -r path; do
    path="$(echo "$path" | sed 's|\\|/|g')"
    if [ ! -f "$path" ]; then
      echo "Missing solution project path: $path" >&2
      missing=1
    fi
  done
}

check_csproj() {
  local csproj="$1"
  while IFS= read -r ref; do
    local normalized
    normalized="$(echo "$ref" | sed 's|\\|/|g')"
    local fullpath
    if [[ "$normalized" == \$\(RepoRoot\)* ]]; then
      normalized="${normalized#\$(RepoRoot)}"
      fullpath="$repo_root/$normalized"
    else
      fullpath="$(dirname "$csproj")/$normalized"
    fi
    if [ ! -f "$fullpath" ]; then
      echo "Missing project reference in $csproj: $normalized" >&2
      missing=1
    fi
  done < <(grep -oP '<ProjectReference\s+Include="\K[^"]+' "$csproj")
}

check_solution

while IFS= read -r csproj; do
  check_csproj "$csproj"
done < <(find . -name '*.csproj')

exit $missing
