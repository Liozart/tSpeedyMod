using GiuxItems.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.World.Generation;

namespace GiuxItems
{
    public class GiuxWorld : ModWorld
    {
        const double giuxOreNmb = 5E-05;
        const float wellNmb = 2200f;
        const float tunnelsNmb = 80f;

        int[] tunnelChestItemsList = { ItemID.PhilosophersStone, ItemID.LuckyHorseshoe, ItemID.BalloonPufferfish, ItemID.PowerGlove, ItemID.FleshKnuckles, ItemID.MoonCharm,
                                    ItemID.MagmaStone, ItemID.FrozenTurtleShell, ItemID.PaladinsShield, ItemID.ShinyRedBalloon, ItemID.Seaweed, ItemID.ObsidianRose,
                                    ItemID.PutridScent, ItemID.SharkToothNecklace, ItemID.Flamelash, ItemID.JellyfishNecklace, ItemID.GoldenFishingRod, ItemID.FishHook, ItemID.FuzzyCarrot };
        int[] wellChestItemsList = { ItemID.DivingGear, ItemID.DivingHelmet, ItemID.SailfishBoots, ItemID.WaterWalkingBoots, ItemID.FrogLeg, ItemID.PhilosophersStone,
                                    ItemID.ClimbingClaws, ItemID.WhoopieCushion, ItemID.NeptunesShell};
        int[] tunnelBonusChestItemsList = { ItemID.SpelunkerPotion, ItemID.TeleportationPotion, ItemID.GreaterHealingPotion, ItemID.MasterBait, ItemID.MythrilBar, ModContent.ItemType<Items.Placeables.BathrGeode>() };

        public override void Initialize()
        { }

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
                // ExampleModOres is a method seen below.
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Giux Ores", GiuxOres));
            }

            //Add wells step
            int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));
            if (LivingTreesIndex != -1)
            {
                tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress)
                {
                    // We can inline the world generation code like this, but if exceptions happen within this code 
                    // the error messages are difficult to read, so making methods is better. This is called an anonymous method.
                    progress.Message = "Making wells and tunnels";
                    MakeWells();
                }));
            }

            //Add mine tunnels step
            int PostIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Spider Caves"));
            if (PostIndex != -1)
                tasks.Insert(PostIndex + 1, new PassLegacy("Mine Tunnels", MakeTunnels));
        }

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
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, Main.maxTilesY); // WorldGen.worldSurfaceLow is actually the highest surface tile. In practice you might want to use WorldGen.rockLayer or other WorldGen values.

                // Then, we call WorldGen.TileRunner with random "strength" and random "steps", as well as the Tile we wish to place. Feel free to experiment with strength and step to see the shape they generate.
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(6, 10), WorldGen.genRand.Next(10, 11), ModContent.TileType<GiuxOre>());

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

        private void MakeWells()
        {
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
                    if (attempts > 1000)
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
            int tunnelHeight = Main.rand.Next(4, 8);
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
                                til.type = 0;
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
                            tilem.type = 0;
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
                        while(bt.active() != true)
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
                                tilem.type = 0;
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
                                til.type = 0;
                                til.active(false);
                            }
                        }
                    }
                }
            }
            return true;
        }

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
    }
}
