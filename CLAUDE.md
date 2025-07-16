# Development Guidelines

This document contains critical information about working with this codebase. Follow these guidelines precisely.

## Core Development Rules

- Don't use public keyword merely for serialization, use [SerializeField] for that purpose.
- Only make fields public when you want to access them outside the script.
- Explicitly write the access modifiers even if they are not required, i.e. private void Update() etc.
- Use underscore for private fields, unless with [SerializeField] use camelCase without underscore.
- Make use of the Events script I wrote in EventModule for global events and observer pattern.
- Make use of the Pools script I wrote in PoolModule for pooling purposes.
- For saving purposes, check SaveModule and scripts under that namespace.
- For tweens, use DOTween package that I installed in the project.
- I am making this game for Steam and added the package Steamworks.Net, make use of it when necessary.
- Keep things scalable but simple, I am an experienced dev so it is not a learning project.
