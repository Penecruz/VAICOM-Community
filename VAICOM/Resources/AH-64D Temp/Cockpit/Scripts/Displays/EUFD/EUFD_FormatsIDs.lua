
local count = -1
local function counter()
	count = count + 1
	return count
end

EUFD_DISPL_FMT_LEV1 =
{
	NONE	= counter(),
	MAIN	= counter(),
	PRESET	= counter(),
	TEST	= counter(),
	LOAD	= counter(),
}

EUFD_DISPL_FMT_LEV2 =
{
	NONE	= 0,
}

EUFD_DISPL_FMT_LEV3 =
{
	NONE	= 0,
}

EUFD_DISPL_FMT_LEV4 =
{
	NONE	= 0,
}