﻿using Mapster.Common.MemoryMappedTypes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Mapster.Rendering;

public static class TileRenderer
{
    public static BaseShape? Tessellate(this MapFeatureData feature, ref BoundingBox boundingBox, ref PriorityQueue<BaseShape, int> shapes)
    {
        BaseShape? baseShape = null;

        var featureType = feature.Type;
		int renderType = (int)feature.RenderType;

		switch ((RenderType)renderType) {
            case RenderType.PLACE_NAME: {
                var coordinates = feature.Coordinates;
                var popPlace = new PopulatedPlace(coordinates, feature);
                baseShape = popPlace;
                shapes.Enqueue(popPlace, popPlace.ZIndex);
                break;
            }

            case RenderType.WATERWAY: {
                var coordinates = feature.Coordinates;
                var waterway = new Waterway(coordinates, feature.Type == GeometryType.Polygon);
                baseShape = waterway;
                shapes.Enqueue(waterway, waterway.ZIndex);
                break;
            }

            case RenderType.LU__N_FOREST: {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Forest);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                break;
            }

            case RenderType.LU__N_PLAIN: {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Plain);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                break;
            }

            case RenderType.LU__N_HILLS: {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Hills);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                break;
            }

            case RenderType.LU__N_MOUNTAINS: {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Mountains);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                break;
            }

            case RenderType.LU__N_DESERT: {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Desert);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                break;
            }

            case RenderType.LU__N_WATER: {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Water);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                break;
            }
 
            case RenderType.LU_R__FOUNTAIN: {
                var coordinates = feature.Coordinates;
                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Water);
                baseShape = geoFeature;
                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                break;
            }           

            default: {
                switch((RenderType)(renderType / 10 * 10)) {
                    case RenderType.LU__LEISURE: {
                        var coordinates = feature.Coordinates;
                        var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Leisure);
                        baseShape = geoFeature;
                        shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                        break;
                    }

                    case RenderType.H__MOTORWAY:
                    case RenderType.H__TRUNK:
                    case RenderType.H__PRIMARY:
                    case RenderType.H__SECONDARY:
                    case RenderType.H__TERTIARY:
                    case RenderType.H__RESIDENTIAL:
                    case RenderType.H__TRACK: {
                        var coordinates = feature.Coordinates;
                        var road = new Road(
                            coordinates, 
                            (RenderType)(renderType / 10 * 10) switch {
                                RenderType.H__MOTORWAY     => RoadType.Motorway,
                                RenderType.H__TRUNK        => RoadType.Trunk,
                                RenderType.H__PRIMARY      => RoadType.Primary,
                                RenderType.H__SECONDARY    => RoadType.Secondary,
                                RenderType.H__TERTIARY     => RoadType.Tertiary,
                                RenderType.H__RESIDENTIAL  => RoadType.Residential,
                                RenderType.H__TRACK        => RoadType.Track,
                                _                          => RoadType.Unknown,
                            }
                        );
                        baseShape = road;
                        shapes.Enqueue(road, road.ZIndex);
                        break;
                    }

                    default: {
                        switch((RenderType)(renderType / 100 * 100)) {
                            case RenderType.LU_RESIDENTIAL: {
                                var coordinates = feature.Coordinates;
                                var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Residential);
                                baseShape = geoFeature;
                                shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                                break;
                            }

                            default: {
                                switch((RenderType)(renderType / 1000 * 1000)) {
                                    case RenderType.UNKNOWN: {
                                        throw new ArgumentException("Unknown type");
                                    }

                                    case RenderType.HIGHWAY: {
                                        var coordinates = feature.Coordinates;
                                        var road = new Road(coordinates);
                                        baseShape = road;
                                        shapes.Enqueue(road, road.ZIndex);
                                        break;
                                    }

                                    case RenderType.RAILWAY: {
                                        var coordinates = feature.Coordinates;
                                        var railway = new Railway(coordinates);
                                        baseShape = railway;
                                        shapes.Enqueue(railway, railway.ZIndex);
                                        break;
                                    }

                                    case RenderType.BORDER: {
                                        var coordinates = feature.Coordinates;
                                        var border = new Border(coordinates);
                                        baseShape = border;
                                        shapes.Enqueue(border, border.ZIndex);
                                        break;
                                    }

                                    case RenderType.LANDUSE: {
                                        var coordinates = feature.Coordinates;
                                        var geoFeature = new GeoFeature(coordinates, GeoFeature.GeoFeatureType.Unknown);
                                        baseShape = geoFeature;
                                        shapes.Enqueue(geoFeature, geoFeature.ZIndex);
                                        break;
                                    }

                                    default: {
                                        Console.Error.WriteLine($"Unimplemented RenderType: {renderType}");
                                        return null;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                break;
            }
		}

        if (baseShape != null)
        {
            for (var j = 0; j < baseShape.ScreenCoordinates.Length; ++j)
            {
                boundingBox.MinX = Math.Min(boundingBox.MinX, baseShape.ScreenCoordinates[j].X);
                boundingBox.MaxX = Math.Max(boundingBox.MaxX, baseShape.ScreenCoordinates[j].X);
                boundingBox.MinY = Math.Min(boundingBox.MinY, baseShape.ScreenCoordinates[j].Y);
                boundingBox.MaxY = Math.Max(boundingBox.MaxY, baseShape.ScreenCoordinates[j].Y);
            }
        }

        return baseShape;
    }

    public static Image<Rgba32> Render(this PriorityQueue<BaseShape, int> shapes, BoundingBox boundingBox, int width, int height)
    {
        var canvas = new Image<Rgba32>(width, height);

        var scaleX = canvas.Width / (boundingBox.MaxX - boundingBox.MinX);
        var scaleY = canvas.Height / (boundingBox.MaxY - boundingBox.MinY);
        var scale = Math.Min(scaleX, scaleY);

        canvas.Mutate(x => x.Fill(Color.White));
        while (shapes.Count > 0)
        {
            var entry = shapes.Dequeue();
            if (entry.ScreenCoordinates.Length < 2)
            {
                continue;
            }
            entry.TranslateAndScale(boundingBox.MinX, boundingBox.MinY, scale, canvas.Height);
            canvas.Mutate(x => entry.Render(x));
        }

        return canvas;
    }

    public struct BoundingBox
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
    }
}
