---
name: Request software release
about: Use this to request a production software release.
title: 'Release Solid Instruments 0.0.0'
labels: 'Category-ProductionRelease, Stage-0-New, Verdict-Pending'
---

# Software Release Request

This issue represents a request for the production release of a new version of **Solid Instruments**.

## Overview

> **MODIFY.** Replace the version number below with the appropriate version number.

Issue a production software release for :label:`v0.0.0`.

## Statement of work

The following list describes the work to be done.

- [ ] Update source documentation to reflect software changes.
- [ ] Update documentation website to reflect software changes.
- [ ] Add release notes with details that reflect software changes.
- [ ] Update :page_facing_up:`appveyor.yml` to reflect new version number.
- [ ] Submit a pull request against the :yellow_circle:`master` branch.
- [ ] Close and destroy the completed working branches.

## Revision control plan

> **MODIFY.** Replace the version number below with the appropriate version number.
>
**Solid Instruments** uses the [**RapidField Revision Control Workflow**](https://github.com/RapidField/solid-instruments/blob/master/CONTRIBUTING.md#revision-control-strategy). Individual contributors should follow the branching plan below when working on this issue.

- :yellow_circle:`master` is the pull request target for
- :purple_circle:`release/v0.0.0`, which is the pull request target for
- :large_blue_circle:`develop`
