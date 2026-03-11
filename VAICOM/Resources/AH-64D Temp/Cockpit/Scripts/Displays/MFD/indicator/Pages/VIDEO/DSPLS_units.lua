-- MDG screen units definitions, also units conversion
-- DI - F/A-18 Multipurpose Display Group stroke indicator Display Increment

function roundDI(value)
	if value > 0 then
		return math.floor(value + 0.5)
	else
		return math.ceil(value - 0.5)
	end
end

MeterToIn         	= 39.3701 / 1.00000054
DegToMil          	= math.rad(1) * 1000
UnitsPerSide      	= 1024
HalfUnitsPerSide  	= UnitsPerSide / 2

local DItoMil_ 		= 0.34
local MilToDI_ 		= 1.0 / DItoMil_

local DItoIn_  		= 0.0048
local InToDI_  		= 1.0 / DItoIn_

local DegToDI_ 		= math.rad(1) * 1000.0 * MilToDI_
local DItoDeg_ 		= 1.0 / DegToDI_

local RadToDI_ 		= 1000.0 * MilToDI_
local DItoRad_ 		= 1.0 / RadToDI_

local DegToRad_		= math.rad(1)
local RadToDeg_		= 1.0 / DegToRad_


-- DI is the minimum possible stroke ray position increment in MDG indicators.
function DItoMil(param) return (param or 1) * DItoMil_ end                      -- DI to milliradians
function MilToDI(param) return (param or 1) * MilToDI_ end                 		-- milliradians to DI

function DItoIn(param) return (param or 1) * DItoIn_ end                        -- DI to inches
function InToDI(param) return (param or 1) * InToDI_ end                       	-- inches to DI

function DegToDI(param) return (param or 1) * DegToDI_ end          			-- degrees to DI
function DItoDeg(param) return (param or 1) * DItoDeg_ end                     	-- DI to degrees

function RadToDI(param) return (param or 1) * RadToDI_ end          			-- radians to DI
function DItoRad(param) return (param or 1) * DItoRad_ end                     	-- DI to radians

function DegToRad(param) return (param or 1) * DegToRad_ end
function RadToDeg(param) return (param or 1) * RadToDeg_ end
