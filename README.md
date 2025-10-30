# RPG Stats

A framework for handling stats and stat modifiers in C# and Unity.

## YouTube Tutorial

Watch the [RPG Stats Playlist](https://youtu.be/VU9FHXjkC8E&list=PLm7W8dbdflogpdc3pcVBiogA8hIjNnGHp) on YouTube for a full, in-depth explanation of the usage and step-by-step implementation of the codebase.

## Installation

### Install via Git URL

1. Navigate to the Packages folder in your Unity project and open the `manifest.json` file.
2. Add this line:
	-	```json
		"com.kryzarel.rpg-stats": "https://github.com/Kryzarel/rpg-stats.git",
		```

### Install manually

1. Clone or download the [RPG Stats](https://github.com/Kryzarel/rpg-stats) repository.
2. Copy/paste or move the whole repository folder directly into your project's Packages folder or into the Assets folder.

## Usage Examples

### Basic Usage

Declare a Stat and (optionally) specify a base value.

```csharp
Stat<StatModifierData> strength = new(10);
```

Declare Stat Modifiers:
```csharp
StatModifier<StatModifierData> modifierAdd = new(5, new StatModifierData(StatModifierType.Add));
StatModifier<StatModifierData> modifierMult = new(0.2f, new StatModifierData(StatModifierType.Mult));
```

Add Modifiers to the Stat:
```csharp
strength.AddModifier(modifierAdd);
strength.AddModifier(modifierMult);
```

Get the Stat's final value, taking into account the base value and all the modifiers ((10 + 5) * 1.2 = 18)
```csharp
float value = strength.FinalValue; // Should be equal to 18
```

## Author

[Kryzarel](https://www.youtube.com/@Kryzarel)

## License

MIT