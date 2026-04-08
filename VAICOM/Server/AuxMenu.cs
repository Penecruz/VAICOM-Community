using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VAICOM.Database;
using VAICOM.FileManager;
using VAICOM.Static;

namespace VAICOM
{
    namespace Servers
    {

        public static partial class Server
        {

            public class ServerAuxmenu
            {
                public string name;
                public List<MenuCommand> items;
                public ServerAuxmenu submenu;
            }

            public class MenuCommand
            {
                public string name;
                public MenuAction command;
                public ServerAuxmenu submenu;

            }

            public class MenuAction
            {
                public int? actionIndex;
            }

            // helper function for iterative scan
            public static int ScanTree(ServerAuxmenu rootMenu)
            {
                int newCommandCounter = 0;

                // Use a stack for iterative traversal
                Stack<ServerAuxmenu> menuStack = new Stack<ServerAuxmenu>();
                menuStack.Push(rootMenu);

                Log.Write("Starting ScanTree method.", Colors.Text);

                while (menuStack.Count > 0)
                {
                    ServerAuxmenu currentMenu = menuStack.Pop();
                    Log.Write($"Processing menu: {currentMenu.name}", Colors.Text);

                    foreach (MenuCommand menuItem in currentMenu.items)
                    {
                        string menuItemName = Helpers.Common.RemoveIllegalCharsForDB(menuItem.name);
                        // Skip entries that consist only of blank text after any illegal characters were removed.
                        if (string.IsNullOrWhiteSpace(menuItemName))
                        {
                            Log.Write($"Ignoring menu item with blank text: '{menuItemName}', original name: '{menuItem.name}'", Colors.Text);
                        }
                        else
                        {
                            string identifier = "Action " + menuItemName;
                            Log.Write($"Processing menu item: {menuItem.name}, Identifier: {identifier}", Colors.Text);

                            // Consolidate checks to minimize redundant lookups
                            if (menuItem.command != null && 
                                !Labels.aicommands.ContainsValue(identifier) &&
                                !Aliases.importedmenus.ContainsKey(identifier))
                            {
                                Log.Write($"Adding new menu item: {identifier}", Colors.Text);

                                // Add new menu item
                                Aliases.importedmenus[identifier] = identifier;

                                MenuItem item = new MenuItem
                                {
                                    menuname = State.menuauxname,
                                    itemname = identifier,
                                    actionIndex = menuItem.command.actionIndex ?? -1,
                                    server = State.menuauxserver
                                };

                                auxmenuitems[identifier] = item;
                                newCommandCounter++;
                            }
                            else if (menuItem.command != null && Aliases.importedmenus.ContainsValue(identifier))
                            {
                                Log.Write($"Updating existing menu item: {identifier}", Colors.Text);

                                // Update action index for existing menu item
                                if (Commands.Table.TryGetValue(identifier, out Command existingCommand))
                                {
                                    existingCommand.actionIndex = menuItem.command.actionIndex ?? existingCommand.actionIndex;
                                }
                            }
                        }

                        // Add submenu to the stack for further processing
                        if (menuItem.submenu != null)
                        {
                            Log.Write($"Adding submenu to stack: {menuItem.submenu.name}", Colors.Text);
                            menuStack.Push(menuItem.submenu);
                        }
                    }

                    // Process the submenu of the current menu
                    if (currentMenu.submenu != null)
                    {
                        Log.Write($"Adding submenu to stack: {currentMenu.submenu.name}", Colors.Text);
                        menuStack.Push(currentMenu.submenu);
                    }
                }

                return newCommandCounter;
            }

            public static void GUI_EditorReflectChanges()
            {
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    try
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.Reflectrequiresreload();
                            State.configurationwindow.Reflectunsavedchanges();
                            State.configurationwindow.SetAliasCounter();
                        });
                    }
                    catch
                    {
                    }
                }
            }

            // extract F10 menu items
            public static void ImportAuxMenu()
            {

                try
                {
                    int newCommandCounter = 0;

                    State.menuauxserver = State.currentstate.missiontitle;
                    State.menuauxname = State.currentstate.menuaux.name;

                    Log.Write($"Mission title: {State.menuauxserver}, Menu name: {State.menuauxname}", Colors.Text);

                    newCommandCounter += ScanTree(State.currentstate.menuaux);

                    if (newCommandCounter > 0)
                    {
                        Log.Write($"New commands found: {newCommandCounter}. Writing to database.", Colors.Text);

                        FileHandler.Database.WriteCategoryToFile("importedmenus", Aliases.importedmenus, true); // write the updates to files
                        FileHandler.Database.WriteCategoryToFile("aicommands", Aliases.aicommands, true);
                        FileHandler.Database.WriteAuxMenuItemsToFile(true);
                        FileHandler.Database.ReadAllCategoriesFromFile(); // reload -> refreshes all keyword tables
                        FileHandler.Database.ReadAuxMenuItemsFromFile();

                        SetImportedMenusAsCommands(); // set new commands
                        Aliases.BuildNewMasterTable(); // update keywords list

                        State.activeconfig.Editorrequiresreload = true;
                        State.activeconfig.Editorunsavedchanges = true;

                        Settings.ConfigFile.WriteConfigToFile(true);
                        Log.Write("New F10 menu commands were imported, the database was updated.", Colors.Warning);

                        GUI_EditorReflectChanges();
                    }
                    else
                    {
                        Log.Write("No new F10 menu items were found.", Colors.Text);
                    }

                    State.menuauximported = false;
                    Log.Write("ImportAuxMenu method completed.", Colors.Text);
                }
                catch (Exception ex)
                {
                    Log.Write($"There was a problem importing the F10 menu: {ex.Message}", Colors.Text);
                }
            }

            public static void SetImportedMenusAsCommands()
            {
                Log.Write("Starting SetImportedMenusAsCommands method.", Colors.Text);

                Aliases.UpdateScanCats();

                if (Aliases.importedmenus != null)
                {
                    int id = 20000;
                    Log.Write($"Adding {Aliases.importedmenus.Count} imported menu commands.", Colors.Text);

                    foreach (KeyValuePair<string, string> menuitem in Aliases.importedmenus)
                    {
                        if (id < 20999) { id = id + 1; }
                        if (!Commands.Table.ContainsKey(menuitem.Value))
                        {
                            Labels.importedmenus.Add(menuitem.Value, menuitem.Value);
                            Commands.Table.Add(menuitem.Value, new Command
                            {
                                actionIndex = auxmenuitems[menuitem.Value].actionIndex,
                                menuitemname = auxmenuitems[menuitem.Value].itemname,
                                servername = auxmenuitems[menuitem.Value].server,
                                category = CommandCategories.auxmenu,
                                uniqueid = id,
                                dcsid = menuitem.Value,
                                displayname = menuitem.Value
                            });

                            Log.Write($"Set menu F10 item: {menuitem.Key}, ActionIndex: {auxmenuitems[menuitem.Value].actionIndex}, Command ID: {id}", Colors.Text);
                        }
                    }
                }

                Log.Write("SetImportedMenusAsCommands method completed.", Colors.Text);
            }
        }
    }
}
