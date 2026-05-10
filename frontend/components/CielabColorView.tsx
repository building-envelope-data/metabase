import Copyable from "./Copyable";
import { CielabColor } from "../__generated__/graphql";
import Float, { Unit } from "./Float";

export default function CielabColorView({ value }: { value: CielabColor }) {
  return (
    <Copyable
      onlyIcon
      text={JSON.stringify(value, (key, value) => {
        if (key === "__typename") return undefined;
        return value;
      })}
    >
      (L* <Float value={value.lStar} unit={Unit.UNITLESS} />, a*{" "}
      <Float value={value.aStar} unit={Unit.UNITLESS} />, b*{" "}
      <Float value={value.bStar} unit={Unit.UNITLESS} />)
    </Copyable>
  );
}
