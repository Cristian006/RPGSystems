﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Systems.ItemSystem;
using Systems.StatSystem;
using Systems.InventorySystem.Utility;

//TODO: Event handlers for weapon change
namespace Systems.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField]
        private Entity entity;
        [SerializeField]
        private Inventory _inventory;

        private int _primaryIndex = -1;
        private int _secondaryIndex = -1;
        private int _tertiaryIndex = -1;

        #region GETTERS AND SETTERS
        private Inventory inventory
        {
            get
            {
                if(_inventory == null)
                {
                    _inventory = new Inventory();
                }
                return _inventory;
            }
            set
            {
                _inventory = value;
            }
        }

        public Entity MyEntity
        {
            get
            {
                if (entity == null)
                {
                    entity = GetComponent<Entity>();
                }
                return entity;
            }
        }

        public int PrimaryIndex
        {
            get
            {
                return _primaryIndex;
            }
            set
            {
                _primaryIndex = value;
            }
        }

        public int SecondaryIndex
        {
            get
            {
                return _secondaryIndex;
            }

            set
            {
                _secondaryIndex = value;
            }
        }

        public int TertiaryIndex
        {
            get
            {
                return _tertiaryIndex;
            }

            set
            {
                _tertiaryIndex = value;
            }
        }
        #endregion

        #region PROPERTIES
        public Weapon Primary
        {
            get
            {
                return _primaryIndex >= 0 ? inventory.Objects<Weapon>().GetAt(_primaryIndex) : null;
            }
        }

        public Weapon Secondary
        {
            get
            {
                return _secondaryIndex >= 0 ? inventory.Objects<Weapon>().GetAt(_secondaryIndex) : null;
            }
        }

        public QuestItem Tertiary
        {
            get
            {
                return _tertiaryIndex >= 0 ? inventory.Objects<QuestItem>().GetAt(_tertiaryIndex) : null;
            }
        }

        public InventoryList<Weapon> Weapons
        {
            get
            {
                return inventory.Weapons;
            }
        }

        public InventoryList<Consumable> Consumables
        {
            get
            {
                return inventory.Consumables;
            }
        }

        public InventoryList<QuestItem> QuestItems
        {
            get
            {
                return inventory.QuestItems;
            }
        }

        /// <summary>
        /// The inventory's current weight
        /// </summary>
        public int CurrentWeight
        {
            get
            {
                return inventory.Weight;
            }
        }

        /// <summary>
        /// The inventory's Max Weight
        /// </summary>
        public int MaxWeight
        {
            get
            {
                return MyEntity.stats.GetStat<StatVital>(StatType.InventoryCap).StatValue;
            }
        }

        /// <summary>
        /// The inventory's available weight
        /// </summary>
        public int AvailableWeight
        {
            get
            {
                return (MaxWeight - CurrentWeight);
            }
        }

        /// <summary>
        /// The respective inventory list count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int Count<T>() where T : Item
        {
            return inventory.Objects<T>().Count;
        }
        #endregion

        #region INVENTORY METHODS
        /// <summary>
        /// Add Item to inventory if there's enough room.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void Add<T>(T item) where T : Item
        {
            if(item.Weight <= AvailableWeight)
            {
                inventory.Objects<T>().Add(item);
            }
            else
            {
                Debug.Log("Can not add anymore Items, too much weight!");
            }
        }

        /// <summary>
        /// Remove Item from the inventory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public void Remove<T>(T item) where T : Item
        {
            inventory.Objects<T>().Remove(item);
        }

        /// <summary>
        /// Remove Item from the inventory at that index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        public void RemoveAt<T>(int index) where T : Item
        {
            inventory.Objects<T>().RemoveAt(index);
        }

        /// <summary>
        /// Replace the Item at that index if the difference in weight of the two items fit in the Inventory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Replace<T>(int index, T item) where T : Item
        {
            int differnceInWeight = inventory.Objects<T>().GetAt(index).Weight - item.Weight;

            if ((inventory.Weight - differnceInWeight) <= MaxWeight)
            {
                inventory.Objects<T>().Replace(index, item);
            }
            else
            {
                Debug.Log("CANNOT REPLACE ITEM, TOO MUCH WEIGHT");
            }
        }

        public bool Contains<T>(T item) where T : Item
        {
            return inventory.Objects<T>().Contains(item);
        }

        public bool Contains<T>(int id) where T : Item
        {
            return inventory.Objects<T>().Contains(id);
        }

        public bool Contains<T>(string name) where T : Item
        {
            return inventory.Objects<T>().Contains(name);
        }

        public T GetAt<T>(int index) where T : Item
        {
            return inventory.Objects<T>().GetAt(index);
        }

        public T GetBy<T>(string name) where T : Item
        {
            return inventory.Objects<T>().GetBy(name);
        }

        public T GetBy<T>(int id) where T : Item
        {
            return inventory.Objects<T>().GetBy(id);
        }
        #endregion

        #region EQUIPPING METHODS
        /// <summary>
        /// Equip an Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="index"></param>
        public void Equip<T>(T item, int index) where T : Item
        {
            Type t = typeof(T);

            if(t == typeof(Weapon))
            {
                if((item as Weapon).WeaponType == WeaponType.Primary)
                {
                    PrimaryIndex = index;
                }
                else
                {
                    SecondaryIndex = index;
                }
            }
            else if (t == typeof(QuestItem))
            {
                TertiaryIndex = index;
            }
            else
            {
                Debug.Log("CANNOT EQUIP THIS TYPE OF ITEM");
            }
        }
        #endregion
    }
}