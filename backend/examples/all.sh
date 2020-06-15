#!/bin/bash

. ./verifyThatIkdbIsEmpty.sh
read -p "Press any key to continue ..."
. ./registerInstitutionsIseAndLbnl.sh
read -p "Press any key to continue ..."
. ./addDatabasesIseAndLbnl.sh
read -p "Press any key to continue ..."
. ./generateComponentIdsAndAddManufacturers.sh
read -p "Press any key to continue ..."
. ./verifyExistenceOfInstitutionsDatabasesAndComponents.sh
read -p "Press any key to continue ..."
. ./whoHasOpticalData.sh
read -p "Press any key to continue ..."
. ./addOpticalDataToLbnlDatabase.sh
read -p "Press any key to continue ..."
. ./addOpticalDataToIseDatabase.sh
read -p "Press any key to continue ..."
. ./whoHasOpticalData.sh
read -p "Press any key to continue ..."
. ./whoHasOpticalDataAtTimestamp.sh
read -p "Press any key to continue ..."
. ./getOpticalDataOfGlazingComponent.sh
