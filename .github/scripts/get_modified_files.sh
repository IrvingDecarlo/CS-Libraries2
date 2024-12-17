#!/bin/bash

# Get arguments
COMMITS_JSON=${1}  # Input argument: The current event's commits converted to JSON.
BRANCH=${2}        # Input argument: Branch to use for getting modified files
FILETYPE=${3}      # Input argument: File type
FILEIGNORE=${4}    # Input argument: File name to skip the modified file.

# Script to get modified .csproj files from the current push's commits
echo -e "\033[36mFetching modified $FILETYPE files in all commits from the current event...\033[0m"

# Output the push's commits.
COMMITS_COUNT=$(echo "$COMMITS_JSON" | jq '. | length')
if [ -z "$COMMITS_COUNT" ] || [ "$COMMITS_COUNT" -eq 0 ]; then
  echo "::error::Unable to determine the amount of commits in the current event."
  exit 1
fi
echo -e "\033[36mCommits in the event ($COMMITS_COUNT):\033[0m"
echo "$COMMITS_JSON" | jq -r '.[].id'

# Ensure the repository is fully fetched to the correct depth
git fetch --prune --unshallow || true
git fetch origin "$BRANCH" --depth="$((COMMITS_COUNT+1))" || true

# Get all modified files
PREVIOUS_SHA=$(git rev-parse HEAD~$COMMITS_COUNT)
CURRENT_SHA=$(echo "$COMMITS_JSON" | jq -r '.[-1].id')
MODIFIED_PROJECTS=$(git diff --name-only $PREVIOUS_SHA..$CURRENT_SHA | grep "$FILETYPE$" | sort | uniq || true)
FILTERED_PROJECTS=""

# Output modified projects and filter by <SkipPublish>
echo -e "\033[36mModified projects:\033[0m"
for project in $MODIFIED_PROJECTS; do
  PROJECT_DIR=$(dirname "$project")
  if [ -f "$PROJECT_DIR/$FILEIGNORE" ]; then
    echo -e "\033[31m$project (skipped)\033[0m"
  else
    FILTERED_PROJECTS="$FILTERED_PROJECTS$project,"
    echo "$project"
  fi
done
FILTERED_PROJECTS=${FILTERED_PROJECTS%,}

# Handle case where no projects are modified
if [ -z "$FILTERED_PROJECTS" ]; then
  echo -e "\033[33mNo modified $FILETYPE files found in the current event.\033[0m"
fi

# Export the result as an environment variable
echo "MODIFIED_PROJECTS=$FILTERED_PROJECTS" >> "$GITHUB_ENV"
