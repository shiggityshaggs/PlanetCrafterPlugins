Sometimes (always?) when duplicating an object which has a LODGroup, the LODs references to the renderers are also duplicated.

This results in messages like the one below, which easily fills log with unnecessary write operations:
"Renderer 'myRenderer' is registered with more than one LODGroup ('someObject' and 'anotherObject')."

The code checks if a renderer is managed by a LODGroup higher up in the object hierarchy.
If so, it removes the offending LOD and destroys the LODGroup if removing the LOD would have left it empty.
