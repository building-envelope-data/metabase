#!/bin/bash
. ./verifyThatMetabaseIsEmpty.sh;                           press_any_key_to "continue" > /dev/null
. ./registerInstitutionsIseAndLbnl.sh;                      press_any_key_to "continue" > /dev/null
. ./addDatabasesIseAndLbnl.sh;                              press_any_key_to "continue" > /dev/null
. ./generateComponentIds.sh;                                press_any_key_to "continue" > /dev/null
. ./addComponentManufacturers.sh;                           press_any_key_to "continue" > /dev/null
. ./verifyExistenceOfInstitutionsDatabasesAndComponents.sh; press_any_key_to "continue" > /dev/null
. ./whoHasOpticalData.sh;                                   press_any_key_to "continue" > /dev/null
. ./addOpticalDataToLbnlDatabase.sh;                        press_any_key_to "continue" > /dev/null
. ./addOpticalDataToIseDatabase.sh;                         press_any_key_to "continue" > /dev/null
. ./whoHasOpticalData.sh;                                   press_any_key_to "continue" > /dev/null
. ./whoHasOpticalDataAtTimestamp.sh;                        press_any_key_to "continue" > /dev/null
. ./getOpticalDataOfGlazingComponent.sh
