---
name: Request software release
about: Use this to request a production software release (project team only).
title: 'Release Solid Instruments 0.0.0'
labels: 'Category-ProductionRelease, Stage-0-New, Verdict-Pending'
---

# Software Release Request

> **:information_source: NOTE.** This issue template is provided for use by **Solid Instruments** team members. Delete this line.

This issue represents a request for the production release of a new version of **Solid Instruments**.

## Overview

> **:pencil2: MODIFY.** Replace the version number below (v0.0.0) with the appropriate version number. Delete this line.

Issue a production software release for :bookmark:`v0.0.0`.

## Statement of work

The following list describes the work to be done.

- [ ] Update relevant source documentation to reflect software changes.
- [ ] Update the [documentation website](https://github.com/RapidField/solid-instruments/tree/master/doc) to reflect software changes.
- [ ] Add [release notes](https://github.com/RapidField/solid-instruments/tree/master/doc/releasenotes) with details that reflect software changes.
- [ ] Update [:page_facing_up:`appveyor.yml`](https://github.com/RapidField/solid-instruments/blob/master/appveyor.yml) to reflect new version number.
- [ ] Submit a pull request against the :yellow_circle:`master` branch.
- [ ] Close and destroy the completed working [branches](https://github.com/RapidField/solid-instruments/branches/all).

## Revision control plan

> **:information_source: PROJECT TEAM:** Update the branch names below to reflect the final plan. Delete this line.

**Solid Instruments** uses the [**RapidField Revision Control Workflow**](https://github.com/RapidField/solid-instruments/blob/master/CONTRIBUTING.md#arrows_clockwise-revision-control-workflow). Individual contributors should follow the branching plan below when working on this issue.

- :yellow_circle:`master` is the pull request target for
  - :orange_circle:`hotfix/v0.0.0` -or-
  - :purple_circle:`release/v0.0.0`, which is the pull request target for
    - :large_blue_circle:`develop`