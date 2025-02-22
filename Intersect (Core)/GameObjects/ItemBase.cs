using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Intersect.Enums;
using Intersect.GameObjects.Conditions;
using Intersect.GameObjects.Events;
using Intersect.Models;
using Intersect.Utilities;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Intersect.GameObjects
{
    public partial class ItemBase : DatabaseObject<ItemBase>, IFolderable
    {
        [NotMapped]
        public ConditionLists UsageRequirements = new ConditionLists();

        public string CannotUseMessage { get; set; } = "";

        public ItemBase()
        {
            Initialize();
        }

        [JsonConstructor]
        public ItemBase(Guid id) : base(id)
        {
            Initialize();
        }

        [Column("Animation")]
        public Guid AnimationId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public AnimationBase Animation
        {
            get => AnimationBase.Get(AnimationId);
            set => AnimationId = value?.Id ?? Guid.Empty;
        }

        [Column("AttackAnimation")]
        [JsonProperty]
        public Guid AttackAnimationId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public AnimationBase AttackAnimation
        {
            get => AnimationBase.Get(AttackAnimationId);
            set => AttackAnimationId = value?.Id ?? Guid.Empty;
        }

        [Column("EquipmentAnimation")]
        public Guid EquipmentAnimationId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public AnimationBase EquipmentAnimation
        {
            get => AnimationBase.Get(EquipmentAnimationId);
            set => EquipmentAnimationId = value?.Id ?? Guid.Empty;
        }

        /// <summary>
        /// Defines whether or not this item can be dropped by a player.
        /// </summary>
        // Not exactly the cleanest solution, since CanDrop and Bound set to true will do the opposite.. But don't want to leave a bogus field!
        [Column("Bound")]
        public bool CanDrop { get; set; } = true;

        /// <summary>
        /// Defines the percentage chance an item will drop upon player Death.
        /// </summary>
        public int DropChanceOnDeath { get; set; }

        /// <summary>
        /// Defines whether or not this item can be traded by a player to another player.
        /// </summary>
        public bool CanTrade { get; set; } = true;

        /// <summary>
        /// Defines whether or not this item can be sold by a player to a shop.
        /// </summary>
        public bool CanSell { get; set; } = true;

        /// <summary>
        /// Defines whether or not this item can be banked by a player.
        /// </summary>
        public bool CanBank { get; set; } = true;

        /// <summary>
        /// Defines whether or not this item can be guild banked by a player.
        /// </summary>
        public bool CanGuildBank { get; set; } = true;

        /// <summary>
        /// Defines whether or not this item can be placed in a bag by a player.
        /// </summary>
        public bool CanBag { get; set; } = true;

        public int CritChance { get; set; }

        public double CritMultiplier { get; set; } = 1.5;

        public int Cooldown { get; set; }

        /// <summary>
        /// Defines which cooldown group this item belongs to.
        /// </summary>
        public string? CooldownGroup { get; set; }

        /// <summary>
        /// Configures whether this should not trigger and be triggered by the global cooldown.
        /// </summary>
        public bool IgnoreGlobalCooldown { get; set; } = false;

        /// <summary>
        /// Configured whether the cooldown of this item should be reduced by the players cooldown reduction
        /// </summary>
        public bool IgnoreCooldownReduction { get; set; } = false;

        public int Damage { get; set; }

        public int DamageType { get; set; }

        public int AttackSpeedModifier { get; set; }

        public int AttackSpeedValue { get; set; }

        public string WeaponSpriteOverride { get; set; }

        public ConsumableData Consumable { get; set; }

        public int EquipmentSlot { get; set; }

        public bool TwoHanded { get; set; }

        public int SlotCount { get; set; }

        [Column("Spell")]
        [JsonProperty]
        public Guid SpellId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public SpellBase Spell
        {
            get => SpellBase.Get(SpellId);
            set => SpellId = value?.Id ?? Guid.Empty;
        }

        public bool QuickCast { get; set; }

        [Column("DestroySpell")]
        public bool SingleUse { get; set; } = true;

        [Column("Event")]
        [JsonProperty]
        public Guid EventId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public EventBase Event
        {
            get => EventBase.Get(EventId);
            set => EventId = value?.Id ?? Guid.Empty;
        }

        public string Description { get; set; } = "";

        public string FemalePaperdoll { get; set; } = "";

        public ItemType ItemType { get; set; }

        public string MalePaperdoll { get; set; } = "";

        public string Icon { get; set; } = "";

        /// <summary>
        /// The database compatible version of <see cref="Color"/>
        /// </summary>
        [Column("Color")]
        [JsonIgnore]
        public string JsonColor
        {
            get => JsonConvert.SerializeObject(Color);
            set => Color = !string.IsNullOrWhiteSpace(value) ? JsonConvert.DeserializeObject<Color>(value) : Color.White;
        }

        /// <summary>
        /// Defines the ARGB color settings for this Item.
        /// </summary>
        [NotMapped]
        public Color Color { get; set; }

        public int Price { get; set; }

        public int Rarity { get; set; }

        [Column("Projectile")]
        [JsonProperty]
        public Guid ProjectileId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ProjectileBase Projectile
        {
            get => ProjectileBase.Get(ProjectileId);
            set => ProjectileId = value?.Id ?? Guid.Empty;
        }

        public int Scaling { get; set; }

        public int ScalingStat { get; set; }

        public int Speed { get; set; }

        public bool Stackable { get; set; }

        /// <summary>
        /// Defines how high the item can stack in a player's inventory before starting a new stack.
        /// </summary>
        public int MaxInventoryStack { get; set; } = 1000000;

        /// <summary>
        /// Defines how high the item can stack in a player/guild's bank before starting a new stack.
        /// </summary>
        public int MaxBankStack { get; set; } = 1000000;

        public int StatGrowth { get; set; }

        public int Tool { get; set; } = -1;

        /// <summary>
        /// Defines the player's chance of successfully defending a hit.
        /// </summary>
        public int BlockChance { get; set; }

        /// <summary>
        /// Sets the damage absorption percentage when successfully defending a hit.
        /// </summary>
        public int BlockAmount { get; set; }

        /// <summary>
        /// Sets the amount of damage absorption to increase the defender's health
        /// </summary>
        public int BlockAbsorption { get; set; }

        /// <summary>
        /// Time in which this item will remain on a map before despawning. Set to 0 to use server configured default value.
        /// </summary>
        public long DespawnTime { get; set; }

        [Column("VitalsGiven")]
        [JsonIgnore]
        public string VitalsJson
        {
            get => DatabaseUtils.SaveIntArray(VitalsGiven, (int) Vital.VitalCount);
            set => VitalsGiven = DatabaseUtils.LoadIntArray(value, (int) Vital.VitalCount);
        }

        [NotMapped]
        public int[] VitalsGiven { get; set; }

        [Column("VitalsRegen")]
        [JsonIgnore]
        public string VitalsRegenJson
        {
            get => DatabaseUtils.SaveIntArray(VitalsRegen, (int) Vital.VitalCount);
            set => VitalsRegen = DatabaseUtils.LoadIntArray(value, (int) Vital.VitalCount);
        }

        [NotMapped]
        public int[] VitalsRegen { get; set; }

        [Column("PercentageVitalsGiven")]
        [JsonIgnore]
        public string PercentageVitalsJson
        {
            get => DatabaseUtils.SaveIntArray(PercentageVitalsGiven, (int) Vital.VitalCount);
            set => PercentageVitalsGiven = DatabaseUtils.LoadIntArray(value, (int) Vital.VitalCount);
        }

        [NotMapped]
        public int[] PercentageVitalsGiven { get; set; }

        [Column("StatsGiven")]
        [JsonIgnore]
        public string StatsJson
        {
            get => DatabaseUtils.SaveIntArray(StatsGiven, (int) Stat.StatCount);
            set => StatsGiven = DatabaseUtils.LoadIntArray(value, (int) Stat.StatCount);
        }

        [NotMapped]
        public int[] StatsGiven { get; set; }

        [Column("PercentageStatsGiven")]
        [JsonIgnore]
        public string PercentageStatsJson
        {
            get => DatabaseUtils.SaveIntArray(PercentageStatsGiven, (int) Stat.StatCount);
            set => PercentageStatsGiven = DatabaseUtils.LoadIntArray(value, (int) Stat.StatCount);
        }

        [NotMapped]
        public int[] PercentageStatsGiven { get; set; }

        [Column("UsageRequirements")]
        [JsonIgnore]
        public string JsonUsageRequirements
        {
            get => UsageRequirements.Data();
            set => UsageRequirements.Load(value);
        }

        [JsonIgnore, NotMapped]
        public bool IsStackable =>
            (ItemType == ItemType.Currency || Stackable)
            && ItemType != ItemType.Equipment
            && ItemType != ItemType.Bag;

        [NotMapped]
        public List<EffectData> Effects { get; set; }

        [Column("Effects")]
        [JsonIgnore]
        public string EffectsJson
        {
            get => JsonConvert.SerializeObject(Effects);
            set => Effects = JsonConvert.DeserializeObject<List<EffectData>>(value ?? "") ?? new List<EffectData>();
        }

        public int GetEffectPercentage(ItemEffect type)
        {
            return Effects.Find(effect => effect.Type == type)?.Percentage ?? 0;
        }

        [NotMapped, JsonIgnore]
        public ItemEffect[] EffectsEnabled
        {
            get => Effects.Select(effect => effect.Type).ToArray();
        }

        public void SetEffectOfType(ItemEffect type, int value)
        {
            var effectToEdit = Effects.Find(effect => effect.Type == type);
            if (effectToEdit == default)
            {
                return;
            }
            effectToEdit.Percentage = value;
        }

        /// <inheritdoc />
        public string Folder { get; set; } = "";

        /// <summary>
        /// Gets an array of all items sharing the provided cooldown group.
        /// </summary>
        /// <param name="cooldownGroup">The cooldown group to search for.</param>
        /// <returns>Returns an array of <see cref="ItemBase"/> containing all items with the supplied cooldown group.</returns>
        public static ItemBase[] GetCooldownGroup(string cooldownGroup)
        {
            cooldownGroup = cooldownGroup.Trim();

            // No point looking for nothing.
            if (string.IsNullOrWhiteSpace(cooldownGroup))
            {
                return Array.Empty<ItemBase>();
            }

            return Lookup
                .Where(i => ((ItemBase)i.Value).CooldownGroup.Trim() == cooldownGroup)
                .Select(i => (ItemBase)i.Value)
                .ToArray();
        }

        private void Initialize()
        {
            Name = "New Item";
            Speed = 10; // Set to 10 by default.
            StatsGiven = new int[(int) Stat.StatCount];
            PercentageStatsGiven = new int[(int) Stat.StatCount];
            VitalsGiven = new int[(int) Vital.VitalCount];
            VitalsRegen = new int[(int) Vital.VitalCount];
            PercentageVitalsGiven = new int[(int) Vital.VitalCount];
            Consumable = new ConsumableData();
            Effects = new List<EffectData>();
            Color = new Color(255, 255, 255, 255);
        }
    }

    [Owned]
    public partial class ConsumableData
    {
        public ConsumableType Type { get; set; }

        public int Value { get; set; }

        public int Percentage { get; set; }
    }

    [Owned]
    public partial class EffectData
    {
        public EffectData()
        {
            Type = default;
            Percentage = default;
        }

        public EffectData(ItemEffect type, int percentage)
        {
            Type = type;
            Percentage = percentage;
        }

        public ItemEffect Type { get; set; }

        public int Percentage { get; set; }
    }
}
