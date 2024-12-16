#!/bin/bash

# Get arguments
COMMITS_JSON=${1}  # Input argument: JSON representation of github.event.commits
BRANCH=${2}        # Input argument: Desired branch name
FILETYPE=${3}      # Input argument: File type

# Script to get modified .csproj files from the current push's commits
echo -e "\033[36mFetching modified $FILETYPE files in all commits from the current push to $BRANCH...\033[0m"

# Output the push's commits.
echo -e "\033[36mCommits in the push:\033[0m"
echo "$COMMITS_JSON" | jq -r '.[].id'

# Get all modified files
MODIFIED_PROJECTS=$(echo "$COMMITS_JSON" | jq -r '.[].modified[]' | grep "\.$FILETYPE$" | sort | uniq || true)
FILTERED_PROJECTS=""

# Output modified projects and filter by <SkipPublish>
echo -e "\033[36mModified projects:\033[0m"
for project in $MODIFIED_PROJECTS; do
  if grep -q "<SkipPublish>True</SkipPublish>" "$project"; then
    echo -e "\033[31m$project (skipped)\033[0m"
  else
    FILTERED_PROJECTS="$FILTERED_PROJECTS$project,"
    echo "$project"
  fi
done
FILTERED_PROJECTS=${FILTERED_PROJECTS%,}

# Handle case where no projects are modified
if [ -z "$FILTERED_PROJECTS" ]; then
  echo -e "\033[33mNo modified $FILETYPE files found in the current push.\033[0m"
fi

# Export the result as an environment variable
echo "MODIFIED_PROJECTS=$FILTERED_PROJECTS" >> "$GITHUB_ENV"
