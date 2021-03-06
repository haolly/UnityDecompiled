﻿namespace UnityEditor
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal static class AssetStoreAssetSelection
    {
        internal static Dictionary<int, AssetStoreAsset> s_SelectedAssets;

        public static void AddAsset(AssetStoreAsset searchResult, Texture2D placeholderPreviewImage)
        {
            <AddAsset>c__AnonStorey1 storey = new <AddAsset>c__AnonStorey1 {
                searchResult = searchResult
            };
            if (placeholderPreviewImage != null)
            {
                storey.searchResult.previewImage = ScaleImage(placeholderPreviewImage, 0x100, 0x100);
            }
            storey.searchResult.previewInfo = null;
            storey.searchResult.previewBundleRequest = null;
            if (!string.IsNullOrEmpty(storey.searchResult.dynamicPreviewURL) && (storey.searchResult.previewBundle == null))
            {
                <AddAsset>c__AnonStorey0 storey2 = new <AddAsset>c__AnonStorey0 {
                    <>f__ref$1 = storey
                };
                storey.searchResult.disposed = false;
                storey2.client = new AsyncHTTPClient(storey.searchResult.dynamicPreviewURL);
                storey2.client.doneCallback = new AsyncHTTPClient.DoneCallback(storey2.<>m__0);
                storey2.client.Begin();
            }
            else if (!string.IsNullOrEmpty(storey.searchResult.staticPreviewURL))
            {
                DownloadStaticPreview(storey.searchResult);
            }
            AddAssetInternal(storey.searchResult);
            RefreshFromServer(null);
        }

        internal static void AddAssetInternal(AssetStoreAsset searchResult)
        {
            if (s_SelectedAssets == null)
            {
                s_SelectedAssets = new Dictionary<int, AssetStoreAsset>();
            }
            s_SelectedAssets[searchResult.id] = searchResult;
        }

        public static void Clear()
        {
            if (s_SelectedAssets != null)
            {
                foreach (KeyValuePair<int, AssetStoreAsset> pair in s_SelectedAssets)
                {
                    pair.Value.Dispose();
                }
                s_SelectedAssets.Clear();
            }
        }

        public static bool ContainsAsset(int id) => 
            ((s_SelectedAssets != null) && s_SelectedAssets.ContainsKey(id));

        private static void DownloadStaticPreview(AssetStoreAsset searchResult)
        {
            <DownloadStaticPreview>c__AnonStorey3 storey = new <DownloadStaticPreview>c__AnonStorey3 {
                searchResult = searchResult
            };
            storey.client = new AsyncHTTPClient(storey.searchResult.staticPreviewURL);
            storey.client.doneCallback = new AsyncHTTPClient.DoneCallback(storey.<>m__0);
            storey.client.Begin();
        }

        public static AssetStoreAsset GetFirstAsset()
        {
            if (s_SelectedAssets == null)
            {
                return null;
            }
            Dictionary<int, AssetStoreAsset>.Enumerator enumerator = s_SelectedAssets.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return null;
            }
            KeyValuePair<int, AssetStoreAsset> current = enumerator.Current;
            return current.Value;
        }

        public static void RefreshFromServer(AssetsRefreshed callback)
        {
            <RefreshFromServer>c__AnonStorey4 storey = new <RefreshFromServer>c__AnonStorey4 {
                callback = callback
            };
            if (s_SelectedAssets.Count != 0)
            {
                List<AssetStoreAsset> assets = new List<AssetStoreAsset>();
                foreach (KeyValuePair<int, AssetStoreAsset> pair in s_SelectedAssets)
                {
                    assets.Add(pair.Value);
                }
                AssetStoreClient.AssetsInfo(assets, new AssetStoreResultBase<AssetStoreAssetsInfo>.Callback(storey.<>m__0));
            }
        }

        private static Texture2D ScaleImage(Texture2D source, int w, int h)
        {
            if ((source.width % 4) != 0)
            {
                return null;
            }
            Texture2D textured2 = new Texture2D(w, h, TextureFormat.RGB24, false, true);
            Color[] pixels = textured2.GetPixels(0);
            double num = 1.0 / ((double) w);
            double num2 = 1.0 / ((double) h);
            double num3 = 0.0;
            double num4 = 0.0;
            int index = 0;
            for (int i = 0; i < h; i++)
            {
                int num7 = 0;
                while (num7 < w)
                {
                    pixels[index] = source.GetPixelBilinear((float) num3, (float) num4);
                    num3 += num;
                    num7++;
                    index++;
                }
                num3 = 0.0;
                num4 += num2;
            }
            textured2.SetPixels(pixels, 0);
            textured2.Apply();
            return textured2;
        }

        public static int Count =>
            ((s_SelectedAssets != null) ? s_SelectedAssets.Count : 0);

        public static bool Empty =>
            ((s_SelectedAssets != null) ? (s_SelectedAssets.Count == 0) : true);

        [CompilerGenerated]
        private sealed class <AddAsset>c__AnonStorey0
        {
            internal AssetStoreAssetSelection.<AddAsset>c__AnonStorey1 <>f__ref$1;
            internal AsyncHTTPClient client;

            internal void <>m__0(AsyncHTTPClient c)
            {
                if (!this.client.IsSuccess())
                {
                    Console.WriteLine("Error downloading dynamic preview: " + this.client.text);
                    this.<>f__ref$1.searchResult.dynamicPreviewURL = null;
                    AssetStoreAssetSelection.DownloadStaticPreview(this.<>f__ref$1.searchResult);
                }
                else
                {
                    AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
                    if ((!this.<>f__ref$1.searchResult.disposed && (firstAsset != null)) && (this.<>f__ref$1.searchResult.id == firstAsset.id))
                    {
                        try
                        {
                            <AddAsset>c__AnonStorey2 storey = new <AddAsset>c__AnonStorey2 {
                                <>f__ref$1 = this.<>f__ref$1,
                                cr = AssetBundle.LoadFromMemoryAsync(c.bytes)
                            };
                            storey.cr.DisableCompatibilityChecks();
                            this.<>f__ref$1.searchResult.previewBundleRequest = storey.cr;
                            storey.callback = null;
                            storey.startTime = EditorApplication.timeSinceStartup;
                            storey.callback = new EditorApplication.CallbackFunction(storey.<>m__0);
                            EditorApplication.update = (EditorApplication.CallbackFunction) Delegate.Combine(EditorApplication.update, storey.callback);
                        }
                        catch (Exception exception)
                        {
                            Console.Write(exception.Message);
                            Debug.Log(exception.Message);
                        }
                    }
                }
            }

            private sealed class <AddAsset>c__AnonStorey2
            {
                internal AssetStoreAssetSelection.<AddAsset>c__AnonStorey1 <>f__ref$1;
                internal EditorApplication.CallbackFunction callback;
                internal AssetBundleCreateRequest cr;
                internal double startTime;

                internal void <>m__0()
                {
                    AssetStoreUtils.UpdatePreloading();
                    if (!this.cr.isDone)
                    {
                        if ((EditorApplication.timeSinceStartup - this.startTime) > 10.0)
                        {
                            EditorApplication.update = (EditorApplication.CallbackFunction) Delegate.Remove(EditorApplication.update, this.callback);
                            if (this.<>f__ref$1.searchResult.dynamicPreviewURL == null)
                            {
                            }
                            Console.WriteLine("Timed out fetch live preview bundle " + "<n/a>");
                        }
                    }
                    else
                    {
                        EditorApplication.update = (EditorApplication.CallbackFunction) Delegate.Remove(EditorApplication.update, this.callback);
                        AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
                        if ((!this.<>f__ref$1.searchResult.disposed && (firstAsset != null)) && (this.<>f__ref$1.searchResult.id == firstAsset.id))
                        {
                            this.<>f__ref$1.searchResult.previewBundle = this.cr.assetBundle;
                            if ((this.cr.assetBundle == null) || (this.cr.assetBundle.mainAsset == null))
                            {
                                this.<>f__ref$1.searchResult.dynamicPreviewURL = null;
                                AssetStoreAssetSelection.DownloadStaticPreview(this.<>f__ref$1.searchResult);
                            }
                            else
                            {
                                this.<>f__ref$1.searchResult.previewAsset = this.<>f__ref$1.searchResult.previewBundle.mainAsset;
                            }
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <AddAsset>c__AnonStorey1
        {
            internal AssetStoreAsset searchResult;
        }

        [CompilerGenerated]
        private sealed class <DownloadStaticPreview>c__AnonStorey3
        {
            internal AsyncHTTPClient client;
            internal AssetStoreAsset searchResult;

            internal void <>m__0(AsyncHTTPClient c)
            {
                if (!this.client.IsSuccess())
                {
                    Console.WriteLine("Error downloading static preview: " + this.client.text);
                }
                else
                {
                    Texture2D texture = c.texture;
                    Texture2D outimage = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false, true);
                    AssetStorePreviewManager.ScaleImage(outimage.width, outimage.height, texture, outimage, null);
                    this.searchResult.previewImage = outimage;
                    UnityEngine.Object.DestroyImmediate(texture);
                    AssetStoreAssetInspector.Instance.Repaint();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RefreshFromServer>c__AnonStorey4
        {
            internal AssetStoreAssetSelection.AssetsRefreshed callback;

            internal void <>m__0(AssetStoreAssetsInfo results)
            {
                AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.ServiceDisabled;
                if ((results.error != null) && (results.error != ""))
                {
                    Console.WriteLine("Error performing Asset Store Info search: " + results.error);
                    AssetStoreAssetInspector.OfflineNoticeEnabled = true;
                    if (this.callback != null)
                    {
                        this.callback();
                    }
                }
                else
                {
                    AssetStoreAssetInspector.OfflineNoticeEnabled = false;
                    if (results.status == AssetStoreAssetsInfo.Status.Ok)
                    {
                        AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.Ok;
                    }
                    else if (results.status == AssetStoreAssetsInfo.Status.BasketNotEmpty)
                    {
                        AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.BasketNotEmpty;
                    }
                    else if (results.status == AssetStoreAssetsInfo.Status.AnonymousUser)
                    {
                        AssetStoreAssetInspector.paymentAvailability = AssetStoreAssetInspector.PaymentAvailability.AnonymousUser;
                    }
                    AssetStoreAssetInspector.s_PurchaseMessage = results.message;
                    AssetStoreAssetInspector.s_PaymentMethodCard = results.paymentMethodCard;
                    AssetStoreAssetInspector.s_PaymentMethodExpire = results.paymentMethodExpire;
                    AssetStoreAssetInspector.s_PriceText = results.priceText;
                    AssetStoreAssetInspector.Instance.Repaint();
                    if (this.callback != null)
                    {
                        this.callback();
                    }
                }
            }
        }

        public delegate void AssetsRefreshed();
    }
}

