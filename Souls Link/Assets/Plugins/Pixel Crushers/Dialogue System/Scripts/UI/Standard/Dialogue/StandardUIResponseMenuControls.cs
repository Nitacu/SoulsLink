// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Manages response menus for StandardDialogueUI.
    /// </summary>
    [System.Serializable]
    public class StandardUIResponseMenuControls : AbstractUIResponseMenuControls
    {

        #region Public Fields

        /// <summary>
        /// Assign this delegate if you want it to replace the default timeout handler.
        /// </summary>
        public System.Action timeoutHandler = null;

        public override AbstractUISubtitleControls subtitleReminderControls { get { return null; } } // Not used.

        #endregion

        #region Private Fields

        protected List<StandardUIMenuPanel> m_builtinPanels = new List<StandardUIMenuPanel>();
        protected StandardUIMenuPanel m_defaultPanel = null;
        protected Dictionary<Transform, StandardUIMenuPanel> m_actorPanelCache = new Dictionary<Transform, StandardUIMenuPanel>();
        protected StandardUIMenuPanel m_currentPanel = null;
        protected Sprite m_pcPortraitSprite = null;
        protected string m_pcPortraitName = null;
        protected bool useFirstResponseForPortrait = false;

        #endregion

        #region Initialization & Lookup

        public void Initialize(StandardUIMenuPanel[] menuPanels, StandardUIMenuPanel defaultMenuPanel, bool useFirstResponseForMenuPortrait)
        {
            m_builtinPanels.Clear();
            m_builtinPanels.AddRange(menuPanels);
            m_defaultPanel = (defaultMenuPanel != null) ? defaultMenuPanel : (m_builtinPanels.Count > 0) ? m_builtinPanels[0] : null;
            ClearCache();
            if (timeoutHandler == null) timeoutHandler = DefaultTimeoutHandler;
            useFirstResponseForPortrait = useFirstResponseForMenuPortrait;
        }

        protected void ClearCache()
        {
            m_actorPanelCache.Clear();
        }

        /// <summary>
        /// Changes a dialogue actor's menu panel for the current conversation.
        /// </summary>
        public virtual void SetActorMenuPanelNumber(DialogueActor dialogueActor, MenuPanelNumber menuPanelNumber)
        {
            if (dialogueActor == null) return;
            OverrideActorMenuPanel(dialogueActor.transform, menuPanelNumber, dialogueActor.standardDialogueUISettings.customMenuPanel);
        }

        /// <summary>
        /// For speakers who do not have DialogueActor components, this method overrides the
        /// actor's default panel.
        /// </summary>
        public void OverrideActorMenuPanel(Transform actorTransform, MenuPanelNumber menuPanelNumber, StandardUIMenuPanel customPanel)
        {
            if (actorTransform == null) return;
            m_actorPanelCache[actorTransform] = GetPanelFromNumber(menuPanelNumber, customPanel);
        }

        protected Transform GetActorTransformFromID(int actorID)
        {
            var actor = DialogueManager.masterDatabase.GetActor(actorID);
            return (actor != null) ? CharacterInfo.GetRegisteredActorTransform(actor.Name) : DialogueManager.currentActor;
        }

        protected virtual StandardUIMenuPanel GetPanel(Subtitle lastSubtitle, Response[] responses)
        {
            // Find player's DialogueActor:
            var playerTransform = (lastSubtitle != null && lastSubtitle.speakerInfo.isPlayer) ? lastSubtitle.speakerInfo.transform
                : (responses != null && responses.Length > 0) ? GetActorTransformFromID(responses[0].destinationEntry.ActorID)
                : DialogueManager.currentActor;
            if (playerTransform == null) playerTransform = DialogueManager.currentActor;

            if (playerTransform == null) return m_defaultPanel;
            if (m_actorPanelCache.ContainsKey(playerTransform)) return m_actorPanelCache[playerTransform];
            var dialogueActor = DialogueActor.GetDialogueActorComponent(playerTransform);

            // Check NPC for non-default menu panel:
            var playerUsesDefaultMenuPanel = dialogueActor == null || dialogueActor.standardDialogueUISettings.menuPanelNumber == MenuPanelNumber.Default;
            var otherTransform = (lastSubtitle != null && lastSubtitle.speakerInfo.isNPC) ? lastSubtitle.speakerInfo.transform : DialogueManager.currentConversant;
            if (playerUsesDefaultMenuPanel && otherTransform != null && m_actorPanelCache.ContainsKey(otherTransform)) return m_actorPanelCache[otherTransform];
            var otherDialogueActor = DialogueActor.GetDialogueActorComponent(otherTransform);
            if (otherDialogueActor != null &&
                (otherDialogueActor.standardDialogueUISettings.useMenuPanelFor == DialogueActor.UseMenuPanelFor.MeAndResponsesToMe ||
                (otherDialogueActor.standardDialogueUISettings.menuPanelNumber != MenuPanelNumber.Default && playerUsesDefaultMenuPanel)))
            {
                if (otherTransform != null && m_actorPanelCache.ContainsKey(otherTransform)) return m_actorPanelCache[otherTransform];
                var otherPanel = GetDialogueActorPanel(otherDialogueActor);
                if (otherPanel != null) return otherPanel;
            }

            // Otherwise use player's menu panel:
            var panel = GetDialogueActorPanel(dialogueActor);
            if (panel == null) panel = m_defaultPanel;
            m_actorPanelCache.Add(playerTransform, panel);
            return panel;
        }

        protected StandardUIMenuPanel GetDialogueActorPanel(DialogueActor dialogueActor)
        {
            if (dialogueActor == null) return null;
            return GetPanelFromNumber(dialogueActor.standardDialogueUISettings.menuPanelNumber, dialogueActor.standardDialogueUISettings.customMenuPanel);
        }

        protected StandardUIMenuPanel GetPanelFromNumber(MenuPanelNumber menuPanelNumber, StandardUIMenuPanel customMenuPanel)
        { 
            switch (menuPanelNumber)
            {
                case MenuPanelNumber.Default:
                    return m_defaultPanel;
                case MenuPanelNumber.Custom:
                    return customMenuPanel;
                default:
                    var index = PanelNumberUtility.GetMenuPanelIndex(menuPanelNumber);
                    return (0 <= index && index < m_builtinPanels.Count) ? m_builtinPanels[index] : null;
            }
        }

        #endregion

        #region Portraits

        /// <summary>
        /// Sets the PC portrait name and sprite to use in the response menu.
        /// </summary>
        /// <param name="portraitSprite">Portrait sprite.</param>
        /// <param name="portraitName">Portrait name.</param>
        public override void SetPCPortrait(Sprite portraitSprite, string portraitName)
        {
            m_pcPortraitSprite = portraitSprite;
            m_pcPortraitName = portraitName;
        }

        /// <summary>
        /// Sets the portrait sprite to use in the response menu if the named actor is the player.
        /// This is used to immediately update the GUI control if the SetPortrait() sequencer 
        /// command changes the portrait sprite.
        /// </summary>
        /// <param name="actorName">Actor name in database.</param>
        /// <param name="portraitSprite">Portrait sprite.</param>
        public override void SetActorPortraitSprite(string actorName, Sprite portraitSprite)
        {
            if (string.Equals(actorName, m_pcPortraitName))
            {
                var actorPortraitSprite = AbstractDialogueUI.GetValidPortraitSprite(actorName, portraitSprite);
                m_pcPortraitSprite = portraitSprite;
                if (m_currentPanel != null && m_currentPanel.pcImage != null && DialogueManager.masterDatabase.IsPlayer(actorName))
                {
                    m_currentPanel.pcImage.sprite = actorPortraitSprite;
                }
            }
        }

        #endregion

        #region Show & Hide Responses 

        protected override void ClearResponseButtons() { } // Unused. Handled by StandardUIMenuPanel.
        protected override void SetResponseButtons(Response[] responses, Transform target) { } // Unused. Handled by StandardUIMenuPanel.

        public override void SetActive(bool value)
        {
            // Only hide. Show is handled by StandardUIMenuPanel.
            if (value == false && m_currentPanel != null) m_currentPanel.HideResponses();
        }

        /// <summary>
        /// Shows a response menu.
        /// </summary>
        /// <param name="lastSubtitle">The last subtitle shown. Used to determine which menu panel to use.</param>
        /// <param name="responses">Responses to show in menu panel.</param>
        /// <param name="target">Send OnClick events to this GameObject (the dialogue UI).</param>
        public override void ShowResponses(Subtitle lastSubtitle, Response[] responses, Transform target)
        {
            var panel = GetPanel(lastSubtitle, responses);
            if (panel == null)
            {
                if (DialogueDebug.logWarnings) Debug.LogWarning("Dialogue System: Can't find menu panel.");
            }
            else
            {
                m_currentPanel = panel;
                if (useFirstResponseForPortrait && responses.Length > 0)
                {
                    var menuCharacterInfo = DialogueManager.conversationModel.GetCharacterInfo(responses[0].destinationEntry.ActorID);
                    if (menuCharacterInfo != null)
                    {
                        m_pcPortraitName = menuCharacterInfo.Name;
                        m_pcPortraitSprite = menuCharacterInfo.portrait;
                    }
                }
                panel.SetPCPortrait(m_pcPortraitSprite, m_pcPortraitName);
                panel.ShowResponses(lastSubtitle, responses, target);
            }
        }

        /// <summary>
        /// Makes the current menu panel's buttons non-clickable.
        /// Typically called by the dialogue UI as soon as a button has been
        /// clicked to make sure the player can't click another one while the
        /// menu is playing its hide animation.
        /// </summary>
        public virtual void MakeButtonsNonclickable()
        {
            if (m_currentPanel != null)
            {
                m_currentPanel.MakeButtonsNonclickable();
            }
        }

        /// <summary>
        /// Close all panels.
        /// </summary>
        public void Close()
        {
            for (int i = 0; i < m_builtinPanels.Count; i++)
            {
                if (m_builtinPanels[i] != null) m_builtinPanels[i].Close();
            }
            if (m_defaultPanel != null && !m_builtinPanels.Contains(m_defaultPanel)) m_defaultPanel.Close();
            foreach (var kvp in m_actorPanelCache)
            {
                var panel = kvp.Value;
                if (panel != null && !m_builtinPanels.Contains(panel)) panel.Close();
            }
            ClearCache();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name='timeout'>Timeout duration in seconds.</param>
        public override void StartTimer(float timeout)
        {
            if (m_currentPanel != null) m_currentPanel.StartTimer(timeout, timeoutHandler);
        }

        public void DefaultTimeoutHandler()
        {
            DialogueManager.instance.SendMessage(DialogueSystemMessages.OnConversationTimeout);
        }

        #endregion

    }

}
