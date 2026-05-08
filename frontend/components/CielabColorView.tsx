import Copyable from "./Copyable";
import { CielabColor } from "../__generated__/graphql";
import Float from "./Float";

export default function CielabColorView({ value }: { value: CielabColor }) {
  return (
    <Copyable
      onlyIcon
      text={JSON.stringify(value, (key, value) => {
        if (key === "__typename") return undefined;
        return value;
      })}
    >
      (L* <Float value={value.lStar} />, a* <Float value={value.aStar} />, b*{" "}
      <Float value={value.bStar} />)
    </Copyable>
  );
}
