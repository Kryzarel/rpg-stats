# RPG Stats

A framework for handling stats and stat modifiers in C# and Unity.

## YouTube Tutorial

Watch the [RPG Stats Playlist](https://www.youtube.com/watch?v=ImmqYBDOspo&list=PLm7W8dbdflogpdc3pcVBiogA8hIjNnGHp) on YouTube for a full, in-depth explanation of the usage and step-by-step implementation of the codebase.

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

## Basic Usage

Import the namespace.
```csharp
using Kryz.RPG.Stats.Default;
```

Declare a Stat and (optionally) specify a base value.
```csharp
Stat strength = new(10);
```

Declare Stat Modifiers.
```csharp
StatModifier<StatModifierData> modifierAdd = new(5, new StatModifierData(StatModifierType.Add));
StatModifier<StatModifierData> modifierMult = new(0.2f, new StatModifierData(StatModifierType.Mult));
```

Add Modifiers to the Stat.
```csharp
strength.AddModifier(modifierAdd);
strength.AddModifier(modifierMult);
```

Get the Stat's final value, which takes into account the base value and all the modifiers ((10 + 5) * 1.2 = 18)
```csharp
float value = strength.FinalValue; // Should be equal to 18
```

## Stat Modifier Types

The `StatModifierType` enum has the following possible values:
1. Add
2. Mult
3. MultTotal
4. Max
5. Min

Modifiers also apply in the order they appear in the bullet points above. A stat's final value is basically calculated by the following formula:
```csharp
private float CalculateFinalValue()
{
	float finalValue = (baseValue + addModifiers) * multModifiers * multTotalModifiers;
	finalValue = Math.Max(finalValue, maxModifiers);
	finalValue = Math.Min(finalValue, minModifiers);
	return finalValue;
}
```

### Add

These modifiers add directly to the value of the stat, i.e., a modifier of `5` will directly increase the stat by `5`, while a modifier of `-5` will reduce the stat by `5`. This is further modified by multiplicative modifiers.

### Mult

Modifiers which apply after `Add` and multiply the value of the stat. If we add a `Mult` modifier with a value of `0.3f`, the stat's final value will increase by `30%`. These modifiers stack additively between themselves, meaning two `0.3f` modifiers will increase the stat by `60%`.

Example:
```csharp
// A stat with the following base value and modifiers:
Stat stat = new(10);
stat.AddModifier(new StatModifier<StatModifierData>(5, new StatModifierData(StatModifierType.Add)));
stat.AddModifier(new StatModifier<StatModifierData>(0.3f, new StatModifierData(StatModifierType.Mult)));
stat.AddModifier(new StatModifier<StatModifierData>(0.3f, new StatModifierData(StatModifierType.Mult)));

// Has a final value of (10 + 5) * (1 + 0.3 + 0.3) = 24
Debug.Assert(stat.FinalValue == 24);
```

The same way a `0.3` modifier increases the stat by `30%` (or multiplies it by `1.3`), a `-0.3` modifier decreases the stat by `30%` (or multiplies it by `0.7`). Two `-0.3` modifiers will decrease the stat by `60%` (or multiply it by `0.4`).

### Mult Total

Modifiers which apply after `Mult` and multiply the value of the stat. The difference between `Mult`and `MultTotal` is that `MultTotal` modifiers stack multiplicatively between themselves, meaning two `0.3f` modifiers will increase the stat by `69%` (nice), because `1.3 * 1.3 = 1.69`.

Example:
```csharp
// A stat with the following base value and modifiers:
Stat stat = new(10);
stat.AddModifier(new StatModifier<StatModifierData>(5, new StatModifierData(StatModifierType.Add)));
stat.AddModifier(new StatModifier<StatModifierData>(0.3f, new StatModifierData(StatModifierType.MultTotal)));
stat.AddModifier(new StatModifier<StatModifierData>(0.3f, new StatModifierData(StatModifierType.MultTotal)));

// Has a final value of (10 + 5) * 1.3 * 1.3 = 25.35
Debug.Assert(stat.FinalValue == 25.35);
```

Since these modifiers stack multiplicatively:
- Two `0.3` modifiers increase the stat by `69%` (or multiply it by `1.69`) -> `1.3 * 1.3 = 1.69`
- Two `-0.3` modifiers decrease the stat by `51%` (or multiply it by `0.49`) -> `0.7 * 0.7 = 0.49`.

### Max

Modifiers which apply after `MultTotal`. These modifiers don't exactly "stack" between themselves, instead the highest of all the `Max` modifiers is chosen, and then the highest between this result and the stat's value is chosen.

Example:
```csharp
// A stat with the following base value and modifiers:
Stat stat = new(10);
stat.AddModifier(new StatModifier<StatModifierData>(5, new StatModifierData(StatModifierType.Add)));
stat.AddModifier(new StatModifier<StatModifierData>(16, new StatModifierData(StatModifierType.Max)));
stat.AddModifier(new StatModifier<StatModifierData>(17, new StatModifierData(StatModifierType.Max)));

// Has a final value of Math.Max(10 + 5, Math.Max(16, 17)) = 17
Debug.Assert(stat.FinalValue == 17);
```

### Min

Modifiers which apply after `Max`. These modifiers don't exactly "stack" between themselves, instead the lowest of all the `Min` modifiers is chosen, and then the lowest between this result and the stat's value is chosen.

Example:
```csharp
// A stat with the following base value and modifiers:
Stat stat = new(10);
stat.AddModifier(new StatModifier<StatModifierData>(5, new StatModifierData(StatModifierType.Add)));
stat.AddModifier(new StatModifier<StatModifierData>(12, new StatModifierData(StatModifierType.Min)));
stat.AddModifier(new StatModifier<StatModifierData>(13, new StatModifierData(StatModifierType.Min)));

// Has a final value of Math.Min(10 + 5, Math.Min(12, 13)) = 12
Debug.Assert(stat.FinalValue == 12);
```

## Removing Modifiers

You can remove modifiers from a `Stat` in multiple ways.

### Direct Instance

The simplest way is passing modifiers you previously declared to the `bool RemoveModifier(StatModifier<T> modifier)` function. This function returns `true` if the modifier was removed successfully.
```csharp
StatModifier<StatModifierData> modifierAdd = new(5, new StatModifierData(StatModifierType.Add));
StatModifier<StatModifierData> modifierMult = new(0.2f, new StatModifierData(StatModifierType.Mult));

// ...

strength.RemoveModifier(modifierAdd);
strength.RemoveModifier(modifierMult);
```

### Remove From Source

An alternative way is declaring your modifiers with a `source`, like so:
```csharp
object source = someObject;
StatModifier<StatModifierData> modifierWithSource = new(5, new StatModifierData(StatModifierType.Add, source));
```

And then calling the `int RemoveAllModifiersFromSource(object)` method, passing in the source object. This will remove all modifiers that belong to the specified source. This function returns the number of removed modifiers.
```csharp
int numRemoved = strength.RemoveAllModifiersFromSource(source);
```

### Custom Stat Modifier Match

The most advanced way is to use the `int RemoveAllModifiers<TMatch>(TMatch)` method. Where `TMatch` can be any object that implements `IEquatable<StatModifier>`.

You can create a custom class or struct that implements the `IEquatable` interface, since `TMatch` is a generic parameter with a `IEquatable` constraint, it should be safe to pass in a struct that implements the interface without causing boxing.
```csharp
public readonly struct RemoveModifiersLargerThan : IEquatable<StatModifier<StatModifierData>>
{
	public readonly float Value;

	public RemoveModifiersLargerThan(float value)
	{
		Value = value;
	}

	public bool Equals(StatModifier<StatModifierData> modifier)
	{
		return modifier.Value > Value;
	}
}
```

In the `Equals()` method, implement custom logic to determine which modifiers to remove. All modifiers in the stat will be passed to the `Equals()` method. When `Equals()` returns `true` for a particular modifier, that modifier will be removed.

You can use the struct like this:
```csharp
strength.RemoveAllModifiers(new RemoveModifiersLargerThan(5));
```

### Built-In Stat Modifier Match

You can also use the built-in struct `StatModifierMatch`, which can be declared like this:
```csharp
StatModifierMatch match = new(modifierValue: floatValue, type: statModifierType, source: sourceObject);
```

Each parameter is optional. If no value is passed to a particular parameter, it won't be used in the `Equals()` comparison at all. If any value is passed, only modifiers with that exact value are removed. If multiple parameters are passed, only modifiers that match ALL parameters are removed.
```csharp
// This removes all modifiers that are of type 'StatModifierType.Add' and have exactly the value 5.
strength.RemoveAllModifiers(new StatModifierMatch(modifierValue: 5, type: StatModifierType.Add));
```

Fun fact, `RemoveAllModifiersFromSource(object)` is actually just a special case of the `RemoveAllModifiers<TMatch>(TMatch)` method, using the built-in `StatModifierMatch` with a source object, like so:
```csharp
public int RemoveModifiersFromSource(object source)
{
	return RemoveAllModifiers(new StatModifierMatch(source: source));
}
```

### Clear

You can also use the `Clear()` method, which just removes all modifiers from the stat.
```csharp
strength.Clear();
```

## Author

[Kryzarel](https://www.youtube.com/@Kryzarel)

## License

MIT