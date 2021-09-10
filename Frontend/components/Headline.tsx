import React from 'react';
import {ChildrenProps} from "../utils";

export function Headline({children}: ChildrenProps) {
  return <>
    <h1>{children}</h1>
    <style jsx>{`
        h1 {
          text-align: center;
          color: var(--primary-main-color);
        }
    `}</style>
  </>;
}