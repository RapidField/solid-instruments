---
name: Report an observed defect
about: Use this to report an unresolved problem that a user is experiencing.
title: ''
labels: 'Category-Defect, Stage-0-New, Verdict-Pending'
---

# Defect Report

This issue represents an unresolved problem that a user is experiencing while using **Solid Instruments**.

## Overview

> **:pencil2: MODIFY.** Replace the text in this section with a clear, concise description of the problem. Place reference details and media in the **Additional information** section, as needed. Delete this line.

Replace this with a summary of the problem.

## Example source

> **:pencil2: MODIFY OR :wastebasket: REMOVE.** Use the section below to demonstrate the problem that the user experienced, or remove this section if it is not needed. Delete this line.

The following example demonstrates the problem.

```csharp
using RapidField.SolidInstruments.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleNamespace
{
    public class ExampleClass
    {
        public void ExampleMethod()
        {
        }
    }

    public class Program
    {
        public void Main(string[] args)
        {
        }
    }
}
```

## Reproduction steps

Follow the instructions below, in order, to reproduce the observed behavior.

> **:pencil2: MODIFY.** Replace the list below with a complete list of reproduction steps. Delete this line.

1. Briefly state the first step.
2. And the second step.
3. And so on...

## Observed (defective) behavior

> **:pencil2: MODIFY.** Replace the text in this section with an explanation of what went wrong. Delete this line.

Replace this with details about the observed result.

## Expected (correct) behavior

> **:pencil2: MODIFY.** Replace the text in this section with a description of what should have happened. Delete this line.

Replace this with details about the expected result.

## System information

> **:pencil2: MODIFY.** Replace the bracketed values below with information about the environment on which the problem was observed. Provide additional system details as necessary. Delete this line.

- Solid Instruments version: [eg. v1.0.0]
- Processor architecture: [eg. x64]
- Operating system: [eg. Windows Server 2016]

## Additional information

> **:pencil2: MODIFY OR :wastebasket: REMOVE.** Place any other information within this section, or remove it. Delete this line.

Replace this with additional details, hyperlinks and/or media.

## Revision control and release plan

> **:no_entry: LEAVE UNMODIFIED.** This section will be completed by the project maintainers after the issue is accepted.
>
> **:information_source: PROJECT TEAM:** Update the version and branch names below to reflect the final plan. Delete these lines.

This defect was or will be resolved as part of the [**:bookmark:v0.0.0**](https://github.com/RapidField/solid-instruments/labels/Version-0.0.0) release.

**Solid Instruments** uses the [**RapidField Revision Control Workflow**](https://github.com/RapidField/solid-instruments/blob/master/CONTRIBUTING.md#arrows_clockwise-revision-control-workflow). Individual contributors should follow the branching plan below when working on this issue.

- :yellow_circle:`master` is the pull request target for
  - :orange_circle:`hotfix/v0.0.0` -or-
  - :purple_circle:`release/v0.0.0`, which is the pull request target for
    - :large_blue_circle:`develop`, which is the pull request target for
      - :red_circle:`defect/00000-xxxxx`, which is the pull request target for the following [user branch(es)](https://github.com/RapidField/solid-instruments/blob/master/CONTRIBUTING.md#brown_circle-user-branches):
        - :brown_circle:`user/{username}/00000-xxxxx`