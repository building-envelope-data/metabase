import Copyable from "./Copyable";
import { CielabColor } from "../__generated__/graphql";

export default function CielabColorViewer({ value }: { value: CielabColor }) {
  return (
    <Copyable
      onlyIcon
      text={JSON.stringify(value, (key, value) => {
        if (key === "__typename") return undefined;
        return value;
      })}
    >
      L*={value.lStar}, a*={value.aStar}, b*={value.bStar}
    </Copyable>
  );
}
