var Direction = { North: 1, East: 2, South: 4, West: 8, Value: [] };

Direction.Value[Direction.North] = "north";
Direction.Value[Direction.East] = "east";
Direction.Value[Direction.South] = "south";
Direction.Value[Direction.West] = "west";
Direction.Value[Direction.North + Direction.East] = "northeast";
Direction.Value[Direction.North + Direction.West] = "northwest";
Direction.Value[Direction.South + Direction.East] = "southeast";
Direction.Value[Direction.South + Direction.West] = "southwest";