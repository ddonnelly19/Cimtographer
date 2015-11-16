﻿using ColossalFramework.UI;
using UnityEngine;
using Mapper.Managers;
using System.Collections.Generic;
using Mapper.CustomUI;
using Mapper.OSM;
using ColossalFramework.Steamworks;

namespace Mapper.Panels
{
    class ExportPanel : UIPanel
    {
        int titleOffset = 40;
        Vector2 offset = Vector2.zero;

        UIButton buttonGenerate = null;

        public override void Awake()
        {
            this.isInteractive = true;
            this.enabled = true;
            this.width = 400;
            this.height = 550;

            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            
            UILabel labelTitle = this.AddUIComponent<UILabel>();
            labelTitle.text = "Cimtographer";
            labelTitle.textScale = 1.3f;
            labelTitle.autoSize = true;
            labelTitle.relativePosition = new Vector3(4, 4);

            CreateTopMessage();
            CreateLeftPanel();
            CreateRightPanel();
            CreateBottomMessage();

            this.relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));
            this.backgroundSprite = "UnlockingPanel2";
            this.atlas = CustomUI.UIUtils.GetAtlas("Ingame");
        }

        private void CreateTopMessage()
        {
            UISprite infoSprite = this.AddUIComponent<UISprite>();
            infoSprite.spriteName = "NotificationIconExtremelyHappy";
            infoSprite.size = new Vector2(32, 32);
            infoSprite.relativePosition = new Vector3(4, 40);

            UILabel label = this.AddUIComponent<UILabel>();
            label.text =    "Oooo, shiny! Cimtographer's had a re-write, and should now work a\n" +
                            "lot better than it did before. Check out the <color#d4fff8>workshop page</color> for\n" +
                            "more information.";
            label.textScale = 0.6f;
            label.size = new Vector2(600, 40);
            label.width = 600;
            label.relativePosition = new Vector2(40, 40);
            label.textColor = new Color32(200, 245, 130, 255);
            label.processMarkup = true;
            label.eventClicked += TopLabel_eventClicked;

            titleOffset += 50;
        }

        private void CreateLeftPanel()
        {
            offset = new Vector2(4, titleOffset);

            CreateOptions();
        }

        private void CreateBottomMessage()
        {
            UISprite infoSprite = this.AddUIComponent<UISprite>();
            infoSprite.spriteName = "IconMessage";
            infoSprite.size = new Vector2(32, 32);
            infoSprite.relativePosition = new Vector3(4, height - 40);

            UILabel label = this.AddUIComponent<UILabel>();
            label.text =    "Remember to post your maps to <color#94c6ff>http://reddit.com/r/cimtographer</color>,\n" +
                            "and show us what you've made!";
            label.textScale = 0.6f;
            label.size = new Vector2(400, 40);
            label.relativePosition = new Vector2(40, height - 32);
            label.processMarkup = true;
            label.eventClicked += BottomLabel_eventClicked;

            UILabel versionLabel = this.AddUIComponent<UILabel>();
            versionLabel.text = "v0.2";
            versionLabel.textScale = 0.4f;
            versionLabel.size = new Vector2(20, 15);
            versionLabel.relativePosition = new Vector2(380, height - 15);
            versionLabel.processMarkup = true;
            versionLabel.textColor = new Color32(180, 180, 180, 255);
        }

        private void CreateRightPanel()
        {
            offset = new Vector2(width / 2, titleOffset);

            buttonGenerate = CustomUI.UIUtils.CreateButton(this);
            buttonGenerate.eventClicked += ButtonGenerate_eventClicked;
            buttonGenerate.textPadding = new RectOffset(6, 6, 6, 6);
            buttonGenerate.size = new Vector2(190, 30);
            buttonGenerate.relativePosition = offset;
            buttonGenerate.text = "Generate OSM map";

            offset += new Vector2(0, buttonGenerate.height + 20);

            UILabel label = this.AddUIComponent<UILabel>();
            label.text =    "<color#94c6ff>Where's the import gone?</color>\n" +
                            "Sorry, but the import functionality\n" +
                            "is something that needs re-developing,\n" +
                            "and doesn't currently work with the\n" +
                            "new code. Since it was the same\n" +
                            "version as the old Cimtographer mod\n" +
                            "you could still use that mod to\n" +
                            "import for now.\n\n" +

                            "<color#94c6ff>Issues?</color>\n" +
                            "If there's any problems with this\n" +
                            "version of the mod, let me know on\n" +
                            "either Reddit or the Workshop.";
            label.textScale = 0.6f;
            label.size = new Vector2(200, 40);
            label.relativePosition = offset;
            label.processMarkup = true;
        }

        private void CreateOptions()
        {
            UIFastList scrollOptionsList = UIFastList.Create<UIOptionItem>(this);
            scrollOptionsList.backgroundSprite = "UnlockingPanel";
            scrollOptionsList.size = new Vector2(192, 350);
            scrollOptionsList.canSelect = true;
            scrollOptionsList.relativePosition = offset;
            scrollOptionsList.rowHeight = 20f;
            scrollOptionsList.rowsData.Clear();

            offset += new Vector2(0, scrollOptionsList.height + 5);

            CreateOptionList(MapperOptionsManager.Instance().exportOptions, scrollOptionsList);

            scrollOptionsList.DisplayAt(0);
            scrollOptionsList.selectedIndex = 0;
        }

        private void CreateOptionList(Dictionary<string, OptionItem> options, UIFastList list = null)
        {
            if (list != null)
            {
                foreach (KeyValuePair<string, OptionItem> option in options)
                {
                    option.Value.id = option.Key;

                    list.rowsData.Add(option.Value);
                }
            }
        }

        private void ButtonGenerate_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            try
            {
                OSMExportNew osmExporter = new OSMExportNew();
                osmExporter.Export();

                buttonGenerate.text = "Generate OSM map";
            }
            catch
            {
                buttonGenerate.text = "Export failed!";
            }
        }

        private void TopLabel_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            Steam.ActivateGameOverlayToWorkshopItem(new PublishedFileId(549792340));
        }

        private void BottomLabel_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            Steam.ActivateGameOverlayToWebPage("https://www.reddit.com/r/Cimtographer");
        }
    }
}
