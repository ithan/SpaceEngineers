#region Prelude
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

// Change this namespace for each script you create.
namespace SpaceEngineers.UWBlockPrograms.BatteryMonitor {
    public sealed class Program : temp {
    // Your code goes between the next #endregion and #region
#endregion

public Program() {
}

public void Main(string args) {
    // container -> press button -> throw list of items (dropper)
    // Find all the Ship connectors
    string[] unwantedItems = {"Stone"};

    var ShipConnectors = new List<IMyTerminalBlock>();  
    GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(ShipConnectors);
    if(ShipConnectors == null) return;  
 
    var Cargo = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(Cargo);
    if(Cargo == null) return;
    
    IMyShipConnector  Dropper    = null;
    
    //Find the dropper 
    var ShipConnectorIndex = 0;
    for(ShipConnectorIndex = 0;ShipConnectorIndex<ShipConnectors.Count;ShipConnectorIndex++){
        if(ShipConnectors[ShipConnectorIndex].BlockDefinition.toString().EndsWith("/ConnectorSmall")){
            Dropper = ShipConnectors[ShipConnectorIndex];
        }
    }


    // Drop all items from all Cargos
    var CargoIndex = 0;
    for(CargoIndex = 0;CargoIndex<Cargo.Count;CargoIndex++){
        var CargoOwner = (IMyInventoryOwner)Cargo[CargoIndex];
        var CargoCube = (IMyCubeBlock)Cargo[CargoIndex];
        var Inventory = (IMyInventory)CargoOwner.GetInventory(0);
        if(CargoCube.IsWorking && CargoCube.IsFunctional && !CargoOwner.GetInventory(0).IsFull) break;

        if(CargoIndex >= Cargo.Count) return; // no empty cargo containers

        var Items = new List<IMyInventoryItem>();
        Items = Inventory.GetItems();
        int i = -1;

        // Find list of items to drop 
        while(Inventory.IsItemAt(++i)){
            if(Array.Exists(unwantedItems, element => element == Items[i].Content.SubtypeName)){
                Inventory.TransferItemTo(Dropper.GetInventory(0), i, null, true, Items[i].Amount);
            }
        }

    }
    
}

#region PreludeFooter
    }
}
#endregion