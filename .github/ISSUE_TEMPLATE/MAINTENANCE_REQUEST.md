---
name: Request project maintenance
about: Use this to request non-functional changes (documentation, testing, refactoring, etc).
title: ''
labels: 'Category-Maintenance, Stage-0-New, Verdict-Pending'
---

# Maintenance Request

This issue represents a request for documentation, testing, refactoring or other non-functional changes.

## Overview

> **:pencil2: MODIFY.** Replace the text in this section with a clear, concise description of the maintenance work. Place reference details and media in the **Additional information** section, as needed. Delete this line.

Replace this with a summary of the requested work.

## Statement of work

The following list describes the work to be done.

> **:pencil2: MODIFY.** Replace the list below with a complete list of work items. Delete this line.

- [ ] Briefly describe the first work item.
- [ ] And the second work item.
- [ ] And so on...

## Additional information

> **:pencil2: MODIFY OR :wastebasket: REMOVE.** Place any other information within this section, or remove it. Delete this line.

Replace this with additional details, hyperlinks and/or media.

## Revision control plan

> **:no_entry: LEAVE UNMODIFIED.** This section will be completed by the project maintainers after the issue is accepted.
>
> **:information_source: PROJECT TEAM:** Update the version and branch names below to reflect the final plan. Delete these lines.

This work was or will be included as part of the [**:bookmark:v0.0.0**](https://github.com/RapidField/solid-instruments/labels/Version-0.0.0) release.

**Solid Instruments** uses the [**RapidField Revision Control Workflow**](https://github.com/RapidField/solid-instruments/blob/master/CONTRIBUTING.md#arrows_clockwise-revision-control-workflow). Individual contributors should follow the branching plan below when working on this issue.

- :yellow_circle:`master` is the pull request target for
  - :purple_circle:`release/v0.0.0`, which is the pull request target for
    - :large_blue_circle:`develop`, which is the pull request target for
      - :black_circle:`maintenance/00000-xxxxx`, which is the pull request target for [user branches](https://github.com/RapidField/solid-instruments/blob/master/CONTRIBUTING.md#brown_circle-user-branches):
        - :brown_circle:`user/{username}/00000-xxxxx`