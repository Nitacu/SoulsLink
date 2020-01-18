// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// An actor asset in a DialogueDatabase. An actor is a participant in a conversation.
    /// </summary>
    [System.Serializable]
    public class Actor : Asset
    {

        /// <summary>
        /// The actor's (optional) portrait image. Corresponds to <c>[pic=1]</c> tag.
        /// </summary>
        public Texture2D portrait = null;

        /// <summary>
        /// Sprite portrait. Allows you to assign a Sprite instead of a full Texture.
        /// </summary>
        public Sprite spritePortrait = null;

        /// <summary>
        /// The alternate portrait images. Corresponds to <c>[pic=2]+</c> tags.
        /// </summary>
        public List<Texture2D> alternatePortraits = new List<Texture2D>();

        /// <summary>
        /// Alternate portrait sprites. Allows you to assign Sprites instead of full Textures.
        /// </summary>
        public List<Sprite> spritePortraits = new List<Sprite>();

        /// <summary>
        /// Gets or sets a value indicating whether this actor is a player character or an NPC.
        /// </summary>
        /// <value>
        /// <c>true</c> if this actor is a player character; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlayer
        {
            get { return LookupBool(DialogueSystemFields.IsPlayer); }
            set { Field.SetValue(fields, DialogueSystemFields.IsPlayer, value); }
        }

        /// <summary>
        /// Gets or sets the texture name to use as the actor's portrait.
        /// </summary>
        /// <value>
        /// The texture name, which is the first item in the Pictures or
        /// Texture Files fields.
        /// </value>
        public string textureName
        {
            get { return LookupTextureName(); }
            set { SetTextureName(value); }
        }

        /// @cond FOR_V1_COMPATIBILITY
        public string TextureName { get { return textureName; } set { textureName = value; } }
        /// @endcond

        /// <summary>
        /// Initializes a new Actor.
        /// </summary>
        public Actor() { }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="sourceActor">Source actor.</param>
        public Actor(Actor sourceActor) : base(sourceActor as Asset)
        {
            this.portrait = sourceActor.portrait;
            this.alternatePortraits = new List<Texture2D>(sourceActor.alternatePortraits);
            this.spritePortrait = sourceActor.spritePortrait;
            this.spritePortraits = new List<Sprite>(sourceActor.spritePortraits);
        }

        /// <summary>
        /// Initializes a new Actor copied from a Chat Mapper actor.
        /// </summary>
        /// <param name='chatMapperActor'>
        /// The Chat Mapper actor.
        /// </param>
        public Actor(ChatMapper.Actor chatMapperActor)
        {
            Assign(chatMapperActor);
        }

        /// <summary>
        /// Copies a Chat Mapper actor.
        /// </summary>
        /// <param name='chatMapperActor'>
        /// The Chat Mapper actor.
        /// </param>
        public void Assign(ChatMapper.Actor chatMapperActor)
        {
            if (chatMapperActor != null) Assign(chatMapperActor.ID, chatMapperActor.Fields);
        }

        /// <summary>
        /// Gets the portrait sprite at a specific index, where <c>1</c> is the default
        /// portrait and <c>2</c>+ are the alternate portraits. Checks the actor's
        /// textures and sprites.
        /// </summary>
        /// <returns>The portrait image.</returns>
        /// <param name="i">The index number of the portrait image.</param>
        public Sprite GetPortraitSprite(int i)
        {
            if (i == 1)
            {
                return UITools.GetSprite(portrait, spritePortrait);
            }
            else
            {
                int index = i - 2;
                return UITools.GetSprite(
                    (0 <= index && index < alternatePortraits.Count) ? alternatePortraits[index] : null,
                    (0 <= index && index < spritePortraits.Count) ? spritePortraits[index] : null);
            }
        }

        public Sprite GetPortraitSprite()
        {
            return UITools.GetSprite(portrait, spritePortrait);
        }

        private string LookupTextureName()
        {
            var field = Field.Lookup(fields, DialogueSystemFields.Pictures);
            if ((field == null) || (field.value == null))
            {
                return null;
            }
            else
            {
                string[] names = field.value.Split(new char[] { '[', ';', ']' });
                return (names.Length >= 2) ? names[1] : null;
            }
        }

        private void SetTextureName(string value)
        {
            Field.SetValue(fields, "Pictures", "[" + value + "]");
        }

    }
}
