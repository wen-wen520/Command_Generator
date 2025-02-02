using Command_Generator.UI.Pages;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Command_Generator.Core.Component
{
    internal class enchantments
    {
//        enchantments
//Specifies the enchantments on this item.

//[NBT Compound / JSON Object] components: Parent tag.
//[NBT Compound / JSON Object] minecraft:enchantments‌[until JE 1.21.5]: Can contain either the following fields, or key-value pairs of levels of enchantments. For the latter, corresponds to [NBT Compound / JSON Object] levels.
//[NBT Compound / JSON Object] levels: Contains key-value pairs of levels of enchantments on this item that affect the way the item works.
//[Int] <enchantment ID>: A single key-value pair, where the key is the resource location of an enchantment, and the value is the level.
//[Boolean] show_in_tooltip: Show or hide enchantments on this item's tooltip. Defaults to .true
//[NBT Compound / JSON Object] minecraft:enchantments​[upcoming JE 1.21.5]: Contains key-value pairs of levels of enchantments on this item that affect the way the item works.
//[Int] <enchantment ID>: A single key-value pair, where the key is the resource location of an enchantment, and the value is the level.
//Example: /give @s wooden_sword[enchantments ={sharpness:3, knockback:2}]
        
        public enchantments() { }

        public void addUnit(string unit_uuid)
        {

        }

    }
}
