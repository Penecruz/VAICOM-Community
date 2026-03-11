---------------------------------- Tools for indicator border mesh handling ----------------------------------
-- Add the second half of the convex poly mirrored about Y axis
function addVertsMirroredAboutY(verts)
	--if #verts == 0 then do return end end
	
	local vertsSz = #verts
	for i = 0, vertsSz - 1 do
		local idx = vertsSz - i
		verts[#verts + 1] = {-(verts[idx][1]), (verts[idx][2])}
	end
end

-- Rescale verts
local function rescaleVerts(verts, scale)
	for i = 1, #verts do
		verts[i] = {verts[i][1] * scale, verts[i][2] * scale}
	end
end

-- Build indices for a convex poly.
-- Mesh should be described point-to-point either clockwise or counterclockwise
-- (both will work, as the poly is usually rendered as a two sided).
local function buildMeshPolyIndices(verts)
	--if #verts < 3 then do return end end
	
	local inds = {}
	for i = 1, #verts - 2 do
		inds[3 * (i-1) + 1] = 0
		inds[3 * (i-1) + 2] = i
		inds[3 * (i-1) + 3] = i + 1 
	end 
	
	return inds
end

function prepareBorderVerts(verts, scale)
	-- Add the second half of the border (mirrored about Y axis)
	addVertsMirroredAboutY(verts)
	-- Rescale vertices to indicator units
	rescaleVerts(verts, scale)
	-- Build indices and return them as a table
	return buildMeshPolyIndices(verts)
end


---------------------------------- Tools for write/read global parameters ----------------------------------
function writeParameter(name, value)
	local param = get_param_handle(name)
	if param ~= nil then
		param:set(value)
	end
end

function readParameter(name)
	local param = get_param_handle(name)
	if param ~= nil then
		return param:get()
	end
	
	return nil
end
