import Copyable from "./Copyable";
import { Scalars } from "../__generated__/graphql";
import { Tooltip } from "antd";

export enum Unit {
  UNITLESS,
  METER,
  WATT_PER_SQUARE_METER_KELVIN,
}

function formatSmartCut(value: number) {
  const string = value.toString();
  const dotIndex = string.indexOf(".");
  if (dotIndex === -1) return string;
  const firstNonZero = string.search(/[1-9]/);
  const significantCut = firstNonZero + 3;
  const minDecimalCut = dotIndex + 4;
  const finalCutIndex = Math.max(significantCut, minDecimalCut);
  if (string.length > finalCutIndex) {
    return string.substring(0, finalCutIndex) + "…";
  }
  return string;
}

const unitLabels = {
  [Unit.METER]: "m",
  [Unit.WATT_PER_SQUARE_METER_KELVIN]: (
    <>
      W·m<sup>-2</sup>·K<sup>-1</sup>
      {/* <span>W/(m<sup>2</sup>·K)</span> */}
    </>
  ),
};

export default function Float({
  value,
  unit,
}: {
  value: Scalars["Float"]["output"];
  unit: Unit;
}) {
  return (
    <Tooltip
      title={
        <Copyable text={value.toString()} color="white">
          <code>{value}</code>
        </Copyable>
      }
      styles={{
        container: {
          whiteSpace: "nowrap",
          minWidth: "max-content",
          maxWidth: "none",
        },
      }}
    >
      <code>
        {formatSmartCut(value)}
        {unit != Unit.UNITLESS && <> {unitLabels[unit]}</>}
      </code>
    </Tooltip>
  );
}
