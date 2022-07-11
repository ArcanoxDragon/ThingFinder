import { EntryObject } from "webpack";
import { discoverProjectEntryPoints } from "./discoverProjectEntryPoints.js";
import { LibraryEntryPoints } from "./LibraryEntryPoints.js";

export async function getEntryPoints(): Promise<EntryObject> {
    const projectEntryPoints = await discoverProjectEntryPoints();

    return {
        ...LibraryEntryPoints,
        ...projectEntryPoints,
    };
}