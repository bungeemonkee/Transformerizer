# Transformerizer

A parallel transformation library with a producer-consumer design

[![Build Status](https://ci.appveyor.com/api/projects/status/j356vaaec24g009q?svg=true)](https://ci.appveyor.com/project/bungeemonkee/transformerizer) [![Coverage Status](https://coveralls.io/repos/github/bungeemonkee/Transformerizer/badge.svg?branch=master)](https://coveralls.io/github/bungeemonkee/Transformerizer?branch=master)

## Description

Transformerizer is a simple transformation library that allows you to run multi-step trasnformations in parallel. Each step can run in parallel with each other using a producer/consumer design and with itself by setting a differnet number of threads to process each step.

## Usage

Usage is simple: any enumerable can begin a transformation by calling the `BeginTransform(..)` extension method. Then call `ThenTransform(...)` as necessary followed by `EndTransform(...)`.

```csharp

var enumerable = new int[] { 1, 2, 3, 4, 5, };

var result = enumerable
    .BeginTransform(x => x * 5, 1)
    .ThenTransform(x => x / 3, 2)
	.EndTransform();
```

## Bugs And Feature Requests

Any TODO items, feature requests, bugs, etc. will be tracked as GitHub isues here:
[https://github.com/bungeemonkee/Transformerizer/issues](https://github.com/bungeemonkee/Transformerizer/issues)
