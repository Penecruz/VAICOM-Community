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
            public static int ScanTree(ServerAuxmenu scanmenu)
            {
                int newcommandcounter = 0;

                foreach (MenuCommand menuitem in scanmenu.items)
                {
                    string identifier = "Action " + Helpers.Common.RemoveIllegalCharsForDB(menuitem.name);

                    if (!(menuitem.command == null)) // there's a command 'action objective blue'
                    {
                        if (!Labels.aicommands.ContainsValue(identifier)) // not an existing command check labels to allow pre loaded aliases?
                        {
                            if (!Aliases.importedmenus.ContainsValue(identifier)) // not a previously imported menu entry
                            {
                                if (!Aliases.importedmenus.ContainsKey(identifier))
                                {
                                    Log.Write("Importing new menu item: " + identifier, Colors.Text);
                                    Log.Write("Action index = " + menuitem.command.actionIndex.ToString(), Colors.Text);
                                    Aliases.importedmenus.Add(identifier, identifier);

                                    MenuItem item = new MenuItem() { menuname = State.menuauxname, itemname = identifier, actionIndex = (int)menuitem.command.actionIndex, server = State.menuauxserver };
                                    auxmenuitems.Add(identifier, item);
                                    newcommandcounter = newcommandcounter + 1;
                                }
                            }
                            else // we have this one already: previously imported. now update its action index.
                            {
                                try
                                {
                                    Commands.Table[identifier].actionIndex = (int)menuitem.command.actionIndex;
                                    Log.Write("F10 entry " + identifier + ": action index updated to value " + menuitem.command.actionIndex.ToString(), Colors.Text);
                                }
                                catch (Exception e)
                                {
                                    Log.Write("F10 entry " + identifier + ": could not update actionindex " + e.Message + e.StackTrace, Colors.Text);
                                }
                            }
                        }
                    }

                    if (!(menuitem.submenu == null))
                    {

                        newcommandcounter = newcommandcounter + ScanTree(menuitem.submenu);
                    }

                }

                if (!(scanmenu.submenu == null))
                {

                    newcommandcounter = newcommandcounter + ScanTree(scanmenu.submenu);
                }

                return newcommandcounter;
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

                    int newcommandcounter = 0;

                    State.menuauxserver = State.currentstate.missiontitle;
                    State.menuauxname = State.currentstate.menuaux.name;

                    newcommandcounter = newcommandcounter + ScanTree(State.currentstate.menuaux);

                    if (newcommandcounter > 0)
                    {
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
                        //Log.Write("No new F10 menu items were found.", colors.Inline);
                    }

                    State.menuauximported = false;

                }
                catch (Exception)
                {
                    Log.Write("There was a problem importing the F10 menu.", Colors.Text);
                }

            }

            // imports stored F10 menu items (called on init)
            public static void SetImportedMenusAsCommands()
            {

                Aliases.UpdateScanCats();

                if (Aliases.importedmenus != null)
                {
                    int id = 20000;
                    Log.Write("Adding " + Aliases.importedmenus.Count.ToString() + " imported menu commands.", Colors.Text);
                    foreach (KeyValuePair<string, string> menuitem in Aliases.importedmenus)
                    {
                        if (id < 20999) { id = id + 1; }
                        if (!Commands.Table.ContainsKey(menuitem.Value))
                        {
                            Labels.importedmenus.Add(menuitem.Value, menuitem.Value);
                            Commands.Table.Add(menuitem.Value, new Command { actionIndex = auxmenuitems[menuitem.Value].actionIndex, menuitemname = auxmenuitems[menuitem.Value].itemname, servername = auxmenuitems[menuitem.Value].server, category = CommandCategories.auxmenu, uniqueid = id, dcsid = menuitem.Value, displayname = menuitem.Value });
                            Log.Write("Setting menu F10 item " + menuitem.Key + " with actionIndex " + auxmenuitems[menuitem.Value].actionIndex + " as command " + id.ToString() + " " + menuitem.Value, Colors.Text);
                        }
                    }
                }
            }

        }
    }
}
