-- VAICOM PRO server-side script
-- ATC.lua (append)
-- https://github.com/Penecruz/VAICOM-Community

local function vaicom_patch_carrier_arrival_progression()
	if dialogsData == nil or dialogsData.dialogs == nil then
		return
	end

	local arrival = dialogsData.dialogs['Arrival']
	if arrival == nil or arrival.stages == nil then
		return
	end

	local readyToLandCarrier = arrival.stages['Ready to land carrier']
	if readyToLandCarrier ~= nil then
		readyToLandCarrier[Message.wMsgATCTowerCopyOverhead] = TO_STAGE('Arrival', 'Ready to land carrier')
		readyToLandCarrier[Message.wMsgLeaderTowerOverhead] = TO_STAGE('Arrival', 'Landing carrier CASE I Clara')
	end

	local readyToLand = arrival.stages['Ready to land']
	if readyToLand ~= nil then
		readyToLand[Message.wMsgATCYouAreClearedForLanding] = TO_STAGE('Arrival', 'Ready to land')
		readyToLand[Message.wMsgATCCheckLandingGear] = TO_STAGE('Arrival', 'Ready to land')
		readyToLand[Message.wMsgLeaderRequestLanding] = TO_STAGE('Arrival', 'Landing')
	end

	local closed = arrival.stages['Closed']
	if closed ~= nil then
		closed[Message.wMsgATCTaxiToParkingArea] = TO_STAGE('Arrival', 'Closed')
		closed[Message.wMsgLeaderRequestTaxiToParking] = TO_STAGE('Arrival', 'Parking')
	end

	local case2and3Approach = arrival.stages['Carrier approach CASE 2 and 3']
	if case2and3Approach ~= nil then
		case2and3Approach[Message.wMsgATCTowerCallTheBall] = TO_STAGE('Arrival', 'Landing carrier CASE I Clara')
		case2and3Approach[Message.wMsgLeaderHornetBall] = TO_STAGE('Arrival', 'Landing carrier CASE I Clara')
		case2and3Approach[Message.wMsgATCTowerSwitchMenu] = TO_STAGE('Arrival', 'Carrier approach CASE 2 and 3')
	end
end

vaicom_patch_carrier_arrival_progression()
