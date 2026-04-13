import { useEffect } from "react";
import { stringifyApolloError } from "../apollo";
import { App } from "antd";
import { ErrorLike } from "@apollo/client";

interface UseQueryHandlerProps {
  error?: ErrorLike | null;
}

export function useQueryHandler({ error }: UseQueryHandlerProps) {
  const { message } = App.useApp();

  useEffect(() => {
    if (error) {
      message.error(stringifyApolloError(error));
    }
  }, [error, message]);
}
