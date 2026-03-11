local count = -1
local function counter()
	count = count + 1
	return count
end

local function reset_counter()
	count = -1
end


reset_counter()
TEDAC_DISPL_FMT_LEV1 =
{
	BLANK		= counter(),
	GRAYSCALE	= counter(),
	FLIGHT		= counter(),
	WEAPON		= counter(),
	FCR			= counter(),
}

reset_counter()
TEDAC_DISPL_FMT_LEV2 =
{
	NONE		= counter(),
	HOVER		= counter(),
	BOB_UP		= counter(),
	TRANSITION	= counter(),
	CRUISE		= counter(),
	WEAPON_TADS	= counter(),
	WEAPON_HMD	= counter(),
	FCR_GTM		= counter(),
	FCR_RMAP    = counter(),
	FCR_ATM    	= counter(),
	FCR_TPM		= counter(),
}

TEDAC_DISPL_FMT_LEV3 =
{
	NONE	= 0,
}

TEDAC_DISPL_FMT_LEV4 =
{
	NONE	= 0,
}