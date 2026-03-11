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