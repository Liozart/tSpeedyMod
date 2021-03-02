using GiuxItems.Tiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace GiuxItems
{
    public class GiuxWorld : ModWorld
    {
        const double giuxOreNmb = 5E-05;
        const float wellNmb = 1600f;
        const float tunnelsNmb = 80f;
        const float castliesNumber = 160f;
        const int bathriteNumber = 10;

        int[] tunnelChestItemsList = { ItemID.PhilosophersStone, ItemID.LuckyHorseshoe, ItemID.BalloonPufferfish, ItemID.PowerGlove, ItemID.FleshKnuckles, ItemID.MoonCharm,
                                    ItemID.MagmaStone, ItemID.FrozenTurtleShell, ItemID.PaladinsShield, ItemID.ShinyRedBalloon, ItemID.Seaweed, ItemID.ObsidianRose,
                                    ItemID.PutridScent, ItemID.SharkToothNecklace, ItemID.Flamelash, ItemID.JellyfishNecklace, ItemID.GoldenFishingRod, ItemID.FishHook, ItemID.FuzzyCarrot,
                                    ItemID.SlimeStaff, ItemID.CandyCornRifle, ItemID.BladedGlove, ItemID.BloodyMachete, ItemID.Cascade, ItemID.BoneSword};
        
        int[] wellChestItemsList = { ItemID.DivingGear, ItemID.DivingHelmet, ItemID.SailfishBoots, ItemID.WaterWalkingBoots, ItemID.FrogLeg, ItemID.PhilosophersStone,
                                    ItemID.ClimbingClaws, ItemID.WhoopieCushion, ItemID.NeptunesShell};
        
        int[] tunnelBonusChestItemsList = { ItemID.SpelunkerPotion, ItemID.TeleportationPotion, ItemID.GreaterHealingPotion, ItemID.MasterBait, ItemID.MythrilBar,
                                            ItemID.CorruptFishingCrate, ItemID.CratePotion, ItemID.CrimsonFishingCrate, ItemID.FloatingIslandFishingCrate, ItemID.GoldenCrate,
                                            ItemID.IronCrate, ItemID.JungleFishingCrate, ItemID.WoodenCrate};

        int[] castliesChestItemList = { ItemID.IlluminantHook, ItemID.InfernoFork, ItemID.TheHorsemansBlade, ItemID.StakeLauncher, ItemID.RavenStaff, ItemID.ChristmasTreeSword,
                                        ItemID.ChainGun, ItemID.BlizzardStaff, ItemID.BatScepter, ItemID.CoinGun, ItemID.BeamSword, ItemID.Marrow, ItemID.Uzi, ItemID.MagicQuiver,
                                        ItemID.DemonHeart, ItemID.BrainScrambler, ItemID.FrostStaff};

        // We use this hook to add 3 steps to world generation at various points. 
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            // Because world generation is like layering several images ontop of each other, we need to do some steps between the original world generation steps.

            // The first step is an Ore. Most vanilla ores are generated in a step called "Shinies", so for maximum compatibility, we will also do this.
            // First, we find out which step "Shinies" is.
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                // Next, we insert our step directly after the original "Shinies" step. 
                // Add Giux Ores
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Giux Ores", GiuxOres));
                //Add hell holes
                tasks.Insert(ShiniesIndex + 2, new PassLegacy("Bathrite ore", Bathrite));
            }

            //Add all structures gen steps after the living tree step 
            int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));
            if (LivingTreesIndex != -1)
            {
                tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Wells", MakeWells));
                //Add mine tunnels step
                tasks.Insert(LivingTreesIndex + 2, new PassLegacy("Mine Tunnels", MakeTunnels));
                //Add lil castlies
                tasks.Insert(LivingTreesIndex + 3, new PassLegacy("Castles", Castlies));
            }
        }

        #region Bathrite

        private void Bathrite(GenerationProgress progress)
        {
            progress.Message = "Spreading Bathrite";
            for (int k = 0; k < bathriteNumber; k++)
            {
                bool success = false;
                int cnt = 0;
                while (!success && cnt < 1000)
                {
                    cnt++;
                    int ranX1 = Main.rand.Next(200, 550);
                    int ranX2 = Main.rand.Next(Main.maxTilesX - 550, Main.maxTilesX - 200);
                    int ranX = (Main.rand.Next(0, 2) == 0) ? ranX1 : ranX2;
                    int ranY = Main.rand.Next(Main.maxTilesY - 80, Main.maxTilesY - 70);
                    if (WorldGen.InWorld(ranX, ranY))
                    {
                        WorldGen.TileRunner(ranX, ranY, 10, 24, ModContent.TileType<Bathrite>(), true, 0, 0, false, true);
                        success = true;
                    }
                    int upY = 0;
                    int min = 0;
                    Tile t = Framing.GetTileSafely(ranX, ranY + upY);
                    while (t.active() || t.lava())
                    {
                        int randt = Main.rand.Next(0, 3);
                        if (min > 10 && randt != 0)
                        {
                            t.type = (ushort)ModContent.TileType<Bathrite>();
                            if (t.lava())
                            {
                                t.lava(false);
                                t.active(true);
                            }
                        }
                        upY++; min++;
                        t = Framing.GetTileSafely(ranX, ranY - upY);
                    }
                }
            }
        }

        #endregion

        #region Castlies

        private void Castlies(GenerationProgress progress)
    {
        progress.Message = "Lil' Castles";
        float widthScale = Main.maxTilesX / castliesNumber;
        int tre = (int)(2f * widthScale);
        int numberToGenerate = WorldGen.genRand.Next((tre / 2) + 1, tre + 1);
        for (int k = 0; k < numberToGenerate; k++)
        {
            bool success = false;
            int cnt = 0;
            while (!success && cnt < 1000)
            {
                cnt++;
                int ranX = Main.rand.Next(200, Main.maxTilesX - 200);
                int ranY = Main.rand.Next((int)WorldGen.rockLayerHigh + 200, Main.maxTilesY - 220);
                if (WorldGen.InWorld(ranX, ranY))
                {
                    //If in a certain biome, leave
                    bool placementOK = true;
                    for (int l = ranX; l < ranX + 11; l++)
                    {
                        for (int m = ranY; m < ranY + 10; m++)
                        {
                            if (Main.tile[l, m].active())
                            {
                                int type = (int)Main.tile[l, m].type;
                                if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick)
                                    placementOK = false;
                            }
                        }
                    }
                    if (!placementOK)
                        break;
                    else
                        success = true;

                    //Create castle tiles and walls
                    for (int y = 0; y < _castleshape.GetLength(0); y++)
                    {
                        for (int x = 0; x < _castleshape.GetLength(1); x++)
                        {
                            int posX = ranX + x;
                            int posY = ranY + y;
                            if (WorldGen.InWorld(posX, posY, 30))
                            {
                                Tile t = Framing.GetTileSafely(posX, posY);
                                switch (_castleshapewalls[y, x])
                                {
                                    case 1:
                                        t.wall = WallID.ChlorophyteBrick;
                                        t.active(false);
                                        break;
                                    case 2:
                                        t.wall = WallID.AdamantiteBeam;
                                        t.active(false);
                                        break;
                                }
                                switch (_castleshape[y, x])
                                {
                                    case 1:
                                        t.type = TileID.GreenDungeonBrick;
                                        t.active(true);
                                        break;
                                    case 2:
                                        t.type = TileID.Traps;
                                        t.frameY = 18;
                                        if (x == 0)
                                            t.frameX = 18;
                                        else
                                            t.frameX = 0;
                                        t.active(true);
                                        break;
                                    case 3:
                                        t.type = TileID.GreenDungeonBrick;
                                        t.active(true);
                                        break;
                                }
                                //Maybe place a beam
                                if (y == 9 && (Main.rand.Next(0, 7) == 0))
                                {
                                    int cntbeam = 1;
                                    Tile tilebe = Framing.GetTileSafely(posX, posY + cntbeam);
                                    while (!tilebe.active())
                                    {
                                        if (cntbeam < 4)
                                            tilebe.type = TileID.WoodenBeam;
                                        else
                                            tilebe.type = TileID.PinkDungeonBrick;
                                        tilebe.active(true);
                                        cntbeam++;
                                        tilebe = Framing.GetTileSafely(posX, posY + cntbeam);
                                    }
                                }
                            }
                        }
                    }
                    //Put entry
                    int entpos = Main.rand.Next(ranX + 1, ranX + 22);
                    for (int en = entpos; en < (entpos + 3); en++)
                    {
                        //Place plaforms
                        Tile tilentry = Framing.GetTileSafely(en, ranY);
                        tilentry.type = TileID.TeamBlockGreenPlatform;
                        //Dig up until theres a empty tile above the platform
                        bool nothing = false;
                        int lessY = 1;
                        while (!nothing)
                        {
                            Tile tile = Framing.GetTileSafely(en, ranY - lessY);
                            if (!tile.active())
                                nothing = true;
                            else
                            {
                                tile.active(false);
                                lessY++;
                            }
                        }
                    }

                    //Then put decorations
                    for (int y = 0; y < _castleshape.GetLength(0); y++)
                    {
                        for (int x = 0; x < _castleshape.GetLength(1); x++)
                        {
                            int posX = ranX + x;
                            int posY = ranY + y;
                            if (WorldGen.InWorld(posX, posY, 30))
                            {
                                switch (_castleshapeDecoration[y, x])
                                {
                                    //Bunny statue
                                    case 1:
                                        WorldGen.PlaceTile(posX, posY, 105, forced:true, style: 9);
                                        break;
                                    //Chest
                                    case 2:
                                        int indi = WorldGen.PlaceChest(posX, posY, 21, false, 15);
                                        if (indi != -1)
                                        {
                                            Chest chestie = Main.chest[indi];
                                            chestie.item[0].SetDefaults(ModContent.ItemType<Items.Placeables.BathrGeode>());
                                            chestie.item[1].SetDefaults(castliesChestItemList[Main.rand.Next(0, castliesChestItemList.Length)]);
                                        }
                                        break;
                                    //Torches
                                    case 3:
                                        //WorldGen.Place1x1(posX, posY, 4, 18);
                                        //WorldGen.PlaceTile(posX, posY, 4, forced: true, style: 18);
                                        break;
                                }
                            }
                        }
                    }
                    //Then put wiring
                    for (int y = 0; y < _castleshape.GetLength(0); y++)
                    {
                        for (int x = 0; x < _castleshape.GetLength(1); x++)
                        {
                            int posX = ranX + x;
                            int posY = ranY + y;
                            if (WorldGen.InWorld(posX, posY, 30))
                            {
                                Tile t = Framing.GetTileSafely(posX, posY);
                                switch (_castleshapeWiring[y, x])
                                {
                                    case 1: t.wire3(true);
                                        break;
                                    case 2: t.wire3(true);
                                        WorldGen.PlaceTile(posX, posY, 135, forced: true, style: 2);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

        #region GiuxOres

        private void GiuxOres(GenerationProgress progress)
        {
            // progress.Message is the message shown to the user while the following code is running. Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes. 
            progress.Message = "Giux Ores";

            // Ores are quite simple, we simply use a for loop and the WorldGen.TileRunner to place splotches of the specified Tile in the world.
            // "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read.
            for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * giuxOreNmb); k++)
            {
                // The inside of this for loop corresponds to one single splotch of our Ore.
                // First, we randomly choose any coordinate in the world by choosing a random x and y value.
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer - 300, Main.maxTilesY); // WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.

                // Then, we call WorldGen.TileRunner with random "strength" and random "steps", as well as the Tile we wish to place. Feel free to experiment with strength and step to see the shape they generate.
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(6, 10), WorldGen.genRand.Next(6, 10), ModContent.TileType<GiuxOre>());

                // Alternately, we could check the tile already present in the coordinate we are interested. Wrapping WorldGen.TileRunner in the following condition would make the ore only generate in Snow.
                // Tile tile = Framing.GetTileSafely(x, y);
                // if (tile.active() && tile.type == TileID.SnowBlock)
                // {
                // 	WorldGen.TileRunner(.....);
                // }
            }
        }

        #endregion

        #region Wells

        private void MakeWells(GenerationProgress progress)
        {
            progress.Message = "Making wells";
            float widthScale = Main.maxTilesX / wellNmb;
            int tre = (int)(2f * widthScale);
            int numberToGenerate = WorldGen.genRand.Next((tre / 2) + 1, tre + 1);
            for (int k = 0; k < numberToGenerate; k++)
            {
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 2000)
                    {
                        success = true;
                        continue;
                    }
                    int i = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
                    if (i <= Main.maxTilesX / 2 - 50 || i >= Main.maxTilesX / 2 + 50)
                    {
                        int j = 0;
                        while (!Main.tile[i, j].active() && (double)j < Main.worldSurface)
                            j++;

                        if (Main.tile[i, j].type == TileID.Dirt)
                        {
                            j--;
                            if (j > 150)
                            {
                                bool placementOK = true;
                                for (int l = i - 4; l < i + 4; l++)
                                {
                                    for (int m = j - 6; m < j + 20; m++)
                                    {
                                        if (Main.tile[l, m].active())
                                        {
                                            int type = (int)Main.tile[l, m].type;
                                            if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick || type == TileID.Cloud || type == TileID.RainCloud)
                                                placementOK = false;
                                        }
                                    }
                                }
                                if (placementOK)
                                    success = PlaceWell(i, j);
                            }
                        }
                    }
                }
            }
        }

        public bool PlaceWell(int i, int j)
        {
            Vector2 chestPos = new Vector2();

            if (!WorldGen.SolidTile(i, j + 1))
            {
                return false;
            }
            if (Main.tile[i, j].active())
            {
                return false;
            }
            if (j < 150)
            {
                return false;
            }

            for (int y = 0; y < _wellshape.GetLength(0); y++)
            {
                for (int x = 0; x < _wellshape.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (_wellshape[y, x])
                        {
                            case 1:
                                tile.type = TileID.LivingWood;
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LivingWood;
                                tile.active(true);
                                tile.halfBrick(true);
                                break;
                            case 3:
                                tile.type = TileID.LivingWood;
                                tile.active(true);
                                tile.slope(2);
                                break;
                            case 4:
                                tile.type = TileID.LivingWood;
                                tile.active(true);
                                tile.slope(1);
                                break;
                            case 5:
                                tile.active(false);
                                break;
                            case 6:
                                tile.type = TileID.VineRope;
                                tile.active(true);
                                break;
                        }
                        switch (_wellshapeWall[y, x])
                        {
                            case 1:
                                tile.wall = WallID.LivingLeaf;
                                break;
                        }

                        switch (_wellshapeWater[y, x])
                        {
                            case 1:
                                tile.liquid = 255;
                                break;
                            case 2:
                                tile.liquid = 255;
                                chestPos.X = k;
                                chestPos.Y = l;
                                break;
                        }
                    }
                }
            }

            //Place chests in the bottom
            int ind = WorldGen.PlaceChest((int)chestPos.X, (int)chestPos.Y, 21, false, 12);
            Chest chest = Main.chest[ind];
            //Place items
            chest.item[0].SetDefaults(wellChestItemsList[Main.rand.Next(0, wellChestItemsList.Length)]);
            chest.item[1].SetDefaults(wellChestItemsList[Main.rand.Next(0, wellChestItemsList.Length)]);

            return true;
        }

        #endregion

        #region Tunnels

        private void MakeTunnels(GenerationProgress progress)
        {
            progress.Message = "Making mines tunnels";

            float widthScale = Main.maxTilesX / tunnelsNmb;
            int tre2 = (int)(2f * widthScale);
            int numberToGenerate = WorldGen.genRand.Next((tre2 / 2) + 1, tre2 + 1);
            for (int k = 0; k < numberToGenerate; k++)
            {
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        success = true;
                        continue;
                    }
                    int i = WorldGen.genRand.Next(300, Main.maxTilesX - 200);
                    int j = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 230);
                    success = PlaceTunnel(i, j);
                }
            }
        }

        public bool PlaceTunnel(int i, int j)
        {
            int tunnelHeight = Main.rand.Next(5, 9);
            //Chest bool
            bool isChestPlaced = false;
            //Create tunnel shapes
            //Init random sizes and espacement of tunnels
            int nmbSegments = Main.rand.Next(2, 6);
            int[] segmentsSizes = new int[nmbSegments];
            for (int n = 0; n < nmbSegments; n++)
                segmentsSizes[n] = Main.rand.Next(6, 28);
            int[] spaceBetweenSegments = new int[nmbSegments - 1];
            int[] directionBetweenSegments = new int[nmbSegments - 1];
            for (int m = 0; m < nmbSegments - 1; m++)
            {
                spaceBetweenSegments[m] = Main.rand.Next(3, 14);
                //negative is up, positive is down
                directionBetweenSegments[m] = (Main.rand.Next(0, 2) == 0) ? -1 : 1;
            }
            //Draw from the inital tile, we take two points and place segments and stairs steps by steps
            int posAX = i, posAY = j, posBX = i, posBY = (j + tunnelHeight);
            for (int seg = 0; seg < nmbSegments; seg++)
            {
                //if first, remove some tiles in front of the tunnel
                if (seg == 0)
                {
                    int width = Main.rand.Next(-7, -3);
                    for (int w = -1; w >= width; w--)
                    {
                        int ranHeight = Main.rand.Next(0, 5);
                        for (int wd = -(ranHeight / 2); wd <= tunnelHeight + (ranHeight / 2); wd++)
                        {
                            if (Main.rand.Next(0, (7 + w)) != 0)
                            {
                                Tile til = Framing.GetTileSafely(posAX + w, posAY + wd);
                                til.active(false);
                            }
                        }
                    }
                }

                for (int cs = 0; cs < segmentsSizes[seg]; cs++)
                {
                    //Place tunnel tiles
                    if (WorldGen.InWorld((int)posAX, posAY, 30) && WorldGen.InWorld((int)posBX, posBY, 30))
                    {
                        Tile tile1 = Framing.GetTileSafely(posAX, posAY);
                        Tile tile2 = Framing.GetTileSafely(posBX, posBY);
                        tile1.type = TileID.BorealWood;
                        tile2.type = TileID.BorealWood;
                        tile1.active(true);
                        tile2.active(true);

                        //Empty the tiles in the tunnel
                        for (int em = (posAY + 1); em < posBY; em++)
                        {
                            Tile tilem = Framing.GetTileSafely(posAX, em);
                            tilem.wall = WallID.BorealWood;
                            tilem.active(false);
                        }
                    }

                    //Possibly place a chest
                    int ind = -1;
                    if (!isChestPlaced)
                        if (Main.rand.Next(0, 55) == 0)
                        {
                            ind = WorldGen.PlaceChest((posBX - 1), (posBY - 1), 21, false, 10);
                            if (ind != -1)
                            {
                                isChestPlaced = true;
                                Chest che = Main.chest[ind];
                                che.item[0].SetDefaults(tunnelChestItemsList[Main.rand.Next(0, tunnelChestItemsList.Length)]);
                                che.item[1].SetDefaults(tunnelChestItemsList[Main.rand.Next(0, tunnelChestItemsList.Length)]);
                                che.item[2].SetDefaults(tunnelBonusChestItemsList[Main.rand.Next(0, tunnelBonusChestItemsList.Length)]);
                                che.item[2].stack = Main.rand.Next(1, 6);
                            }
                        }

                    //Possibly place a beam all the way down
                    if (Main.rand.Next(0, 4) == 0)
                    {
                        int cnt = 1;
                        Tile bt = Framing.GetTileSafely(posBX, posBY + 1);
                        while(!bt.active())
                        {
                            cnt++;
                            bt.type = TileID.WoodenBeam;
                            bt.active(true);
                            bt = Framing.GetTileSafely(posBX, posBY + cnt);
                        }
                    }

                    posAX++;
                    posBX++;
                }
                //Place stairs
                if (seg != (nmbSegments - 1))
                {
                    for (int st = 0; st < spaceBetweenSegments[seg]; st++)
                    {
                        posAY += directionBetweenSegments[seg];
                        posBY += directionBetweenSegments[seg];
                        if (WorldGen.InWorld((int)posAX, posAY, 30) && WorldGen.InWorld((int)posBX, posBY, 30))
                        {
                            Tile tile1 = Framing.GetTileSafely(posAX, posAY);
                            Tile tile2 = Framing.GetTileSafely(posBX, posBY);
                            tile1.type = TileID.BorealWood;
                            tile2.type = TileID.BorealWood;
                            tile1.active(true);
                            tile2.active(true);

                            //Empty the tiles in the stairs
                            for (int em = (posAY + 1); em < posBY; em++)
                            {
                                Tile tilem = Framing.GetTileSafely(posAX, em);
                                tilem.wall = WallID.BorealWood;
                                tilem.active(false);
                            }
                        }
                        posAX++;
                        posBX++;
                    }
                }
                //Else remove some tiles in front of the tunnel
                else
                {
                    int width = Main.rand.Next(3, 8);
                    for (int w = 0; w <= width; w++)
                    {
                        int ranHeight = Main.rand.Next(0, 4);
                        for (int wd = -(ranHeight / 2); wd <= tunnelHeight + (ranHeight / 2); wd++)
                        {
                            if (Main.rand.Next(0, (7 - w)) != 0)
                            {
                                Tile til = Framing.GetTileSafely(posAX + w, posAY + wd);
                                til.active(false);
                            }
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region WellShapes

        private readonly int[,] _wellshape = {
            {0,0,3,1,4,0,0 },
            {0,3,1,1,1,4,0 },
            {3,1,1,1,1,1,4 },
            {5,5,5,6,5,5,5 },
            {5,5,5,6,5,5,5 },
            {5,5,5,6,5,5,5 },
            {2,1,5,6,5,1,2 },
            {1,1,5,5,5,1,1 },
            {1,1,5,5,5,1,1 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,5,5,5,1,0 },
            {0,1,1,1,1,1,0 },
        };
        private readonly int[,] _wellshapeWall = {
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
        };
        private readonly int[,] _wellshapeWater = {
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,1,1,0,0 },
            {0,0,1,2,1,0,0 },
            {0,0,0,0,0,0,0 },
        };

    #endregion

        #region CastliesShapes

    private readonly int[,] _castleshape = {
            {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };

        private readonly int[,] _castleshapewalls = {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,2,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,2,1,1},
            {1,1,2,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,2,1,1},
            {1,1,2,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,2,1,1},
            {1,1,2,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,2,1,1},
            {1,1,2,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,2,1,1},
            {1,1,2,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,2,1,1},
            {1,1,2,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,2,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        private readonly int[,] _castleshapeDecoration = {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,3,0,0,0,0,3,0,0,0,0,0,0,3,0,0,0,0,3,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,0,0,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        private readonly int[,] _castleshapeWiring = {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        #endregion
    }
}
