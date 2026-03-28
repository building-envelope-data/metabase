import { OpenEndedDateTimeRange } from "../__generated__/graphql";
import OpenEndedDateTimeRangeX from "./OpenEndedDateTimeRangeX";

export default function Availability({
  range,
}: {
  range: OpenEndedDateTimeRange;
}) {
  return (
    <div>
      Available <OpenEndedDateTimeRangeX range={range} />
    </div>
  );
}
