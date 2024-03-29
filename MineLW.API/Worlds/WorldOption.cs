﻿using System.Numerics;
using MineLW.API.Math;

namespace MineLW.API.Worlds
{
    public abstract class WorldOption
    {
        public static readonly WorldOption<int> Time = new WorldOption<int>(0);
        public static readonly WorldOption<bool> DaylightCycle = new WorldOption<bool>(false);
        public static readonly WorldOption<bool> Raining = new WorldOption<bool>(false);

        public static readonly WorldOption<WorldEnvironment> Environment = new WorldOption<WorldEnvironment>(
            WorldEnvironment.Normal
        );

        public static readonly WorldOption<Vector3> SpawnPosition = new WorldOption<Vector3>(new Vector3(
            0,
            Minecraft.Units.World.Height / 2f,
            0
        ));

        public static readonly WorldOption<Rotation> SpawnRotation = new WorldOption<Rotation>(
            Rotation.Zero
        );
    }

    public class WorldOption<T> : WorldOption
    {
        public readonly T DefaultValue;

        public WorldOption(T defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}